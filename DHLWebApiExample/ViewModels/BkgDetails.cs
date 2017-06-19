using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHLWebApiExample.ViewModels
{
    public class BkgDetails
    {
        public string PaymentCountryCode { get; set; }
        public string Date { get; set; }
        public string ReadyTime { get; set; }
        public string ReadyTimeGMTOffset { get; set; }

        public string DimensionUnit { get; set; }
        public string WeightUnit { get; set; }
        public float ShipmentWeight { get; set; }
        public int Volume { get; set; }

        public List<Piece> Pieces { get; set; }
        public string PaymentAccountNumber { get; set; }
        public string IsDutiable { get; set; }

    }
}
