using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHLWebApiExample.ViewModels
{
    public class ResponseShipmentInfo
    {
        public string OriginServiceArea_ServiceAreaCode { get; set; }
        public string OriginServiceArea_Description { get; set; }

        public string DestinationServiceArea_ServiceAreaCode { get; set; }
        public string DestinationServiceArea_Description { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAccountNumber { get; set; }
        public string ConsigneeName { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int Pieces { get; set; }
        public float Weight { get; set; }
        public string WeightUnit { get; set; }
        public string GlobalProductCode { get; set; }
        public string ShipmentDesc { get; set; }
        public string DlvyNotificationFlag { get; set; }
        public ResponseShipper Shipper { get; set; }
        public ResponseConsignee Consignee { get; set; }
        public List<ResponseShipmentEvent> ShipmentEvents { get; set; }
    }
}
