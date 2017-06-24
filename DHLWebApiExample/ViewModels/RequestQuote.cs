using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHLWebApiExample.ViewModels
{
    public class RequestQuote : RequestBase
    {
        public RequestQuote()
        {

        }
        public CountryPostalCode Origin { get; set; }
        public CountryPostalCode Destination { get; set; }
        public BkgDetails BkgDetails { get; set; }
        public Dutiable Dutiable { get; set; }
    }
}
