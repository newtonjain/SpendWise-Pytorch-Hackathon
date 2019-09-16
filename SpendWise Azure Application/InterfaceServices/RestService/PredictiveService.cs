using InterfaceServices.RestService.Interfaces;
using ResourceModel;
using ResourceModel.PredictiveService;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using ResourceModel.Suggestions;
using System.Linq;
using System.Collections;

namespace InterfaceServices.RestService
{
    public class PredictiveService : IPredictiveService
    {
        public PredictiveForecastServiceResponse GetDailyPredictiveItem(PrecdictiveForecastServiceRequest predictiveServiceRequest)
        {
            var precdictiveForecastServiceResponse = new PredictiveForecastServiceResponse();

            string apiUrl = "https://spendwise-prediction.herokuapp.com/category";
            //string inputJson = (new JavaScriptSerializer()).Serialize(predictiveServiceRequest);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";


            client.Encoding = Encoding.UTF8;
            string json = client.DownloadString(apiUrl);
            precdictiveForecastServiceResponse.DailyPredictiveItem = (new JavaScriptSerializer()).Deserialize<List<PredictiveItem>>(json);
            return precdictiveForecastServiceResponse;
        }

        public PredictiveForecastServiceResponse GetPredictiveForecast(PrecdictiveForecastServiceRequest predictiveServiceRequest)
        {
            var precdictiveForecastServiceResponse = new PredictiveForecastServiceResponse();
            string result = string.Empty;

            //using (var client = new HttpClient())
            //{
            //    //const string apiKey = "eCXHJn+ovrhVvuue7wLvy4k27z5XmwkytUMrLAeYdz3hju4uaWP4rF+/HOxJQ1dPtmYQciy96z9OP79XsZdnvQ=="; // Replace this with the API key for the web service
            //    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            //    client.BaseAddress = new Uri("https://spendwise-prediction.herokuapp.com/predict?userID=1");


            //    //HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

            //    //var response = client.PostAsync("", new StringContent(
            //    //new JavaScriptSerializer().Serialize(""), Encoding.UTF8, "application/json")).Result;

            //    var response = client.GetAsync("https://spendwise.azurewebsites.net/predict?userID=1").Result;


            //    if (response.IsSuccessStatusCode)
            //    {
            //        result = response.Content.ReadAsStringAsync().Result;
            //        Console.WriteLine("Result: {0}", result);
            //    }
            //    else
            //    {
            //        Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

            //        // Print the headers - they include the requert ID and the timestamp,
            //        // which are useful for debugging the failure
            //        Console.WriteLine(response.Headers.ToString());

            //        //string responseContent = await response.Content.ReadAsStringAsync();
            //        //Console.WriteLine(responseContent);
            //    }
            //}



            //string apiUrl = "http://spendwiseservicemain.azurewebsites.net/api/PredictiveForecast/GetPredictiveForecastExpense";
            string apiUrl = "https://spendwise-prediction.herokuapp.com/predict?userID=1";

            //string inputJson = (new JavaScriptSerializer()).Serialize(predictiveServiceRequest);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "text/plain";
            //client.Encoding = Encoding.UTF8;
            string json = client.DownloadString(apiUrl);

            var objects = JArray.Parse(json); // parse as array 

            List<PredictiveForecast> listforecast = new List<PredictiveForecast>();

            foreach (JObject root in objects)
            {
                PredictiveForecast forecast = new PredictiveForecast();
                foreach (KeyValuePair<String, JToken> app in root)
                {
                    if (app.Key.Equals("spending"))
                    {
                        forecast.Spending = Convert.ToInt32(Math.Round(Convert.ToDouble((String)app.Value)));
                    }

                    if (app.Key.Equals("time"))
                    {
                        forecast.time = Convert.ToDateTime(app.Value.ToString()).ToString("MM/dd");
                    }

                    //listforecast.Add(

                    //    new PredictiveForecast {
                    //        Spending = Convert.ToInt32(Math.Round(Convert.ToDouble((String)app.Value))),
                    //        time = (Convert.ToDateTime (app.Key["time"]))
                    //    }

                    //    );



                    //precdictiveForecastServiceResponse.PredictiveForecast[count].Saving = Convert.ToInt32(Math.Round(Convert.ToDouble((String)app.Value)));

                    //var spending = precdictiveForecastServiceResponse.PredictiveForecast[count].Spending;

                    //precdictiveForecastServiceResponse.PredictiveForecast[count].Spending = Convert.ToInt32(Math.Round(Convert.ToDouble(spending)));

                }

                listforecast.Add(forecast);
            }


            //precdictiveForecastServiceResponse.PredictiveForecast = (new JavaScriptSerializer()).Deserialize<List<PredictiveForecast>>(json);
            precdictiveForecastServiceResponse.PredictiveForecast = listforecast;

            //apiUrl = "http://spendwiseservicemain.azurewebsites.net/api/PredictiveForecast/GetPredictiveForecastSavings";
            apiUrl = "https://spendwise-prediction.herokuapp.com/savingPrediction?days=30";
            json = client.DownloadString(apiUrl);


            objects = JArray.Parse(json); // parse as array  
            foreach (JObject root in objects)
            {
                int count = 0;
                foreach (KeyValuePair<String, JToken> app in root)
                {
                    precdictiveForecastServiceResponse.PredictiveForecast[count].Saving = Convert.ToInt32(Math.Round(Convert.ToDouble((String)app.Value)));

                    var spending = precdictiveForecastServiceResponse.PredictiveForecast[count].Spending;

                    precdictiveForecastServiceResponse.PredictiveForecast[count].Spending = Convert.ToInt32(Math.Round(Convert.ToDouble(spending)));
                    count++;
                }
            }

            return precdictiveForecastServiceResponse;

        }

        public List<Suggestions> GetSuggestions(string productCategory)
        {
           // string apiUrl = string.Format("https://spendwise-prediction.herokuapp.com/localbusinesssearch?query={0}%2008854", productCategory);
           string apiUrl = "https://spendwise-prediction.herokuapp.com/localbusinesssearch?query=coffee%2008854";

            //string inputJson = (new JavaScriptSerializer()).Serialize(predictiveServiceRequest);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "text/plain";
            //client.Encoding = Encoding.UTF8;
            string json = client.DownloadString(apiUrl);

            var dict = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(json);
            List<Suggestions> listSuggestions = new List<Suggestions>();

            var places = (Dictionary<string, object>)dict["places"];

            foreach (Dictionary<string, object> item in places["value"] as IEnumerable)
            {
                var suggestion = new Suggestions();
                foreach (KeyValuePair<string, object> obj in item)
                {
                    if (obj.Key == "name")
                    {
                        suggestion.Name = obj.Value.ToString();
                    }

                    if (obj.Key == "url")
                    {
                        suggestion.URL = obj.Value.ToString();
                    }

                    if (obj.Key == "address")
                    {

                        foreach (KeyValuePair<string, object> address in obj.Value as IEnumerable)
                        {
                            if (address.Key.Equals("text"))
                            {
                                suggestion.StreetAddress = address.Value.ToString();
                            }
                        }
                    }
                }

                listSuggestions.Add(suggestion);
            }
            return listSuggestions;
        }

        public PredictiveServiceResponse GetPredictiveItemsAsync(PredictiveServiceRequest predictiveServiceRequest)
        {

            var predictiveServiceResponse = new PredictiveServiceResponse();
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("http://localhost:55159/");
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            //    HttpResponseMessage response = await client.GetAsync("api/values");
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var items = await response.Content.ReadAsAsync<List<PredictiveItem>>();
            //        //Console.WriteLine("{0}\t${1}\t{2}", item.ItemID, item.ItemName, item.Code);
            //    }
            //}

            string apiUrl = "http://spendwiseservicemain.azurewebsites.net/api/PredictiveForecast/GetPredictiveForecast";
            string inputJson = (new JavaScriptSerializer()).Serialize(predictiveServiceRequest);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";

            client.Encoding = Encoding.UTF8;
            string json = client.DownloadString(apiUrl);
            predictiveServiceResponse.PredictiveItems = (new JavaScriptSerializer()).Deserialize<List<PredictiveItem>>(json);
            return predictiveServiceResponse;
        }

        public string GetPredictiveSavings(PrecdictiveForecastServiceRequest predictiveServiceRequest)
        {
            string apiUrl = "http://spendwiseservicemain.azurewebsites.net/api/PredictiveForecast/GetPredictiveSavings";
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string json = client.DownloadString(apiUrl);
            return json;
        }

        //public PredictiveServiceResponse GetPredictiveItemsAsync(PredictiveServiceRequest predictiveServiceRequest)
        //{
        //    var predictiveServiceResponse = new PredictiveServiceResponse();
        //    //using (var client = new HttpClient())
        //    //{
        //    //    client.BaseAddress = new Uri("http://localhost:55159/");
        //    //    client.DefaultRequestHeaders.Accept.Clear();
        //    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        //    //    HttpResponseMessage response = await client.GetAsync("api/values");
        //    //    if (response.IsSuccessStatusCode)
        //    //    {
        //    //        var items = await response.Content.ReadAsAsync<List<PredictiveItem>>();
        //    //        //Console.WriteLine("{0}\t${1}\t{2}", item.ItemID, item.ItemName, item.Code);
        //    //    }
        //    //}

        //    string apiUrl = "http://spendwiseservicev1.azurewebsites.net/api/PredictiveItems";
        //    string inputJson = (new JavaScriptSerializer()).Serialize(predictiveServiceRequest);
        //    WebClient client = new WebClient();
        //    client.Headers["Content-type"] = "application/json";
        //    client.Encoding = Encoding.UTF8;
        //    string json = client.DownloadString(apiUrl);
        //    predictiveServiceResponse.PredictiveItems = (new JavaScriptSerializer()).Deserialize<List<PredictiveItem>>(json);
        //    return predictiveServiceResponse;
        //}




    }
}
