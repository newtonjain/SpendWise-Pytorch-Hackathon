using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceModel.PredictiveService
{
    public class PredictiveForecastServiceResponse
    {
        public List<PredictiveForecast> PredictiveForecast { get; set; }

        public List<PredictiveItem> DailyPredictiveItem { get; set; }
    }
}
