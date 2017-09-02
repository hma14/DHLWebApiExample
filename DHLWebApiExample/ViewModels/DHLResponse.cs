using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHLWebApiExample.ViewModels
{
    public class DHLResponse
    {
        public REQUESTS RequestType { get; set; }
        public List<ResponseAWBInfo> Trackings { get; set; }
        public ResponseQuote Quote { get; set; }

    }
}
