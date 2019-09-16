using ResourceModel;
using ResourceModel.PredictiveService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceServices.RestService.Interfaces
{
    public interface IPredictiveService
    {
        PredictiveServiceResponse GetPredictiveItemsAsync(PredictiveServiceRequest predictiveServiceRequest);

        PredictiveForecastServiceResponse GetPredictiveForecast(PrecdictiveForecastServiceRequest predictiveServiceRequest);

        string GetPredictiveSavings(PrecdictiveForecastServiceRequest predictiveServiceRequest);

        PredictiveForecastServiceResponse GetDailyPredictiveItem(PrecdictiveForecastServiceRequest predictiveServiceRequest);
    }
}
