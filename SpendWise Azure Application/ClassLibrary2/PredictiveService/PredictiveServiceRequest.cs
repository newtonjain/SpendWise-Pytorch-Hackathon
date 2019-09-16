using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceModel
{
    public class PredictiveServiceRequest
    {
        /// <summary>
        /// Gets or sets the account number
        /// </summary>
        public string CurrentDate { get; set; }

        /// <summary>
        /// Gets or sets the routing number
        /// </summary>
        public string UserId { get; set; }
    }
}
