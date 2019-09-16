using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceModel
{
    public class PredictiveItem
    {
        public int ItemID { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
        public string ItemCategory { get; set; }
        public string ItemAmount { get; set; }
    }
}
