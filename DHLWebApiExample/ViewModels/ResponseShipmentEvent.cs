using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHLWebApiExample.ViewModels
{
    public class ResponseShipmentEvent
    {
        public string Date { get; set; }
        public string Time { get; set; }

        public string ServiceEvent_EventCode { get; set; }
        public string ServiceEvent_Description { get; set; }
        public string Signatory { get; set; }
        public string ServiceArea_ServiceAreaCode { get; set; }
        public string ServiceArea_Description { get; set; }
    }
}
