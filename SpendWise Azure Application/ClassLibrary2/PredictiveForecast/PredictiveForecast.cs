using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceModel
{
    public class PredictiveForecast
    {        
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public string time { get; set; }
        public int Spending { get; set; }
        public int Saving { get; set; }
    }

}
