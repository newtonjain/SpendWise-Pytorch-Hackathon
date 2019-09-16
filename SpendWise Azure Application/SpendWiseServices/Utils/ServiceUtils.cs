using InterfaceServices.RestService;
using ResourceModel;
using ResourceModel.PredictiveService;
using ResourceModel.Suggestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendWiseServices.Utils
{
    public static class ServiceUtils
    {
        public static List<PredictiveItem> GetPredictiveItem(string userId)
        {
            PredictiveService service = new PredictiveService();

            PredictiveServiceRequest request = new PredictiveServiceRequest
            {
                CurrentDate = DateTime.Now.ToShortDateString(),
                UserId = userId
            };

            return service.GetPredictiveItemsAsync(request).PredictiveItems;
        }

        public static List<PredictiveForecast> GetPredictiveForecast(string userId)
        {
            PredictiveService service = new PredictiveService();

            PrecdictiveForecastServiceRequest request = new PrecdictiveForecastServiceRequest
            {
                UserId = userId
            };

            return service.GetPredictiveForecast(request).PredictiveForecast;
        }

        public static List<PredictiveItem> GetDailyPredictiveItems(string userId)
        {
            PredictiveService service = new PredictiveService();

            PrecdictiveForecastServiceRequest request = new PrecdictiveForecastServiceRequest
            {
                UserId = userId
            };

            return service.GetDailyPredictiveItem(request).DailyPredictiveItem;
        }

        public static string GetPredictiveSavings(string userId)
        {
            PredictiveService service = new PredictiveService();

            PrecdictiveForecastServiceRequest request = new PrecdictiveForecastServiceRequest
            {
                UserId = userId
            };

            return service.GetPredictiveSavings(request);
        }

        public static List<Suggestions> GetSuggestions(string productCategory)
        {
            PredictiveService service = new PredictiveService();

            return service.GetSuggestions(productCategory);
        }
    }
}
