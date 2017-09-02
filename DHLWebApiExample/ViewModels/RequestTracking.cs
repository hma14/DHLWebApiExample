using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHLWebApiExample.ViewModels
{
    public class RequestTracking : RequestBase
    {
        public string LanguageCode { get; set; }
        public string Waybill { get; set; }
        public string LPNumber { get; set; }
        public string LevelOfDetails { get; set; }
        public string PiecesEnabled { get; set; }

    }
}
