using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHLWebApiExample.ViewModels
{
    public class Piece
    {
        public int PieceID { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public int Width { get; set; }
        public float Weight { get; set; }
    }
}
