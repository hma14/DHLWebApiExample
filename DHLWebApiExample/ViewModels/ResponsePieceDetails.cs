using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHLWebApiExample.ViewModels
{
    public class ResponsePieceDetails
    {
        public string AWBNumber { get; set; }
        public string LicensePlate { get; set; }
        public int PieceNumber { get; set; }
        public float ActualDepth { get; set; }
        public float ActualWidth { get; set; }
        public float ActualHeight { get; set; }
        public float ActualWeight { get; set; }
        public float Depth { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public string PackageType { get; set; }
        public float DimWeight { get; set; }
        public string WeightUnit { get; set; }
    }
}
