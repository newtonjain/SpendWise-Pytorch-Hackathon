using ResourceModel;
using ResourceModel.Suggestions;
using SpendWise.AI.Models;
using System.Collections.Generic;

namespace SpendWise.Domain
{
    public static class SpendWiseServiceUtil
    {
        public static List<ResourceModel.PredictiveItem> GetPredictiveItem()
        {
            return SpendWiseServices.Utils.ServiceUtils.GetPredictiveItem("abcd");
        }

        public static List<ResourceModel.PredictiveForecast> GetPredictiveForecast()
        {
            return SpendWiseServices.Utils.ServiceUtils.GetPredictiveForecast("1");
        }

        public static string GetPredictiveSavings()
        {
            return SpendWiseServices.Utils.ServiceUtils.GetPredictiveSavings("1");
        }

        public static List<DailyPredictiveViewModel> GetDailyPredictiveItems()
        {
            var model = new List<DailyPredictiveViewModel>();

            var responseDailyPredictiveItem =  SpendWiseServices.Utils.ServiceUtils.GetDailyPredictiveItems("1");


            foreach(PredictiveItem item in responseDailyPredictiveItem)
            {
                model.Add(new DailyPredictiveViewModel
                    {
                    AmountSpend = item.ItemAmount,
                    ItemCategory = item.ItemCategory,
                    ItemID = item.ItemID,
                    ItemName = item.ItemName,
                    ItemImage = GetItemImage(item)
                    
                });
            }
            return model;
        }

        public static List<SuggestionsViewModel> GetSuggestions(string productCategory)
        {
            var model = new List<SuggestionsViewModel>();

            var responseDailyPredictiveItem = SpendWiseServices.Utils.ServiceUtils.GetSuggestions(productCategory);


            foreach (Suggestions item in responseDailyPredictiveItem)
            {
                model.Add(new SuggestionsViewModel
                {
                    Name = item.Name,
                    StreetAddress = item.StreetAddress,
                    URL = item.URL
                });
            }
            return model;
        }

        private static string GetItemImage(PredictiveItem item)
        {
            var imageNumber = string.Empty;
            switch (item.ItemCategory)
            { 
                case "Meals":
                    imageNumber = "sandwich.png";
                    break;
                case "Coffee":
                    imageNumber = "coffee.png";
                    break;
                case "Transportation":
                    imageNumber = "transport.png";
                    break;
                case "Shopping":
                    imageNumber ="cart.png";
                    break;
                case "Entertainment":
                    imageNumber = "popcorn.png";
                    break;
                default:
                    imageNumber = "5";
                    break;
            }

            return imageNumber;
        }
    }
}