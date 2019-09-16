using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpendWise.AI.Models
{
    public class DailyPredictiveViewModel
    {
        public string ItemName { get; set; }
        public int ItemID { get; set; }
        public string AmountSpend { get; set; }
        public string Suggestion { get; set; }
        public string ItemImage { get; internal set; }
        public string ItemCategory { get; internal set; }
    }
}