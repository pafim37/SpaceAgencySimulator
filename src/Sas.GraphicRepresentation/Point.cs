using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.GraphicRepresentation
{
    internal class Point
    {
        private int _x;
        private int _y;

        public int X { get; set; }
        public int Y { get; set; }

        public int GetX() => _x;
        public int GetY() => _y;

        public int R { get; private set; }

        public Point(int x, int y, int r = 10)
        {
            _x = x;
            _y = y;
            R = r;
        }


    }
}
