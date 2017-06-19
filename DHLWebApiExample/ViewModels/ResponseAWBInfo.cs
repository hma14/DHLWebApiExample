using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHLWebApiExample.ViewModels
{
    public class ResponseAWBInfo 
    {
        public string AWBNumber { get; set; }
        public string TrackedBy_LPNumber { get; set; }
        public string Status_ActionStatus { get; set; }
        public ResponseShipmentInfo ShipmentInfo { get; set; }
        public ReponsePieceDetails PieceDetails { get; set; }
    }
}
