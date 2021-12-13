using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Orbit.Frames
{
    public class CenterMassFrame
    {
        public double X { get; set; } 
        public double Y { get; set; }
        public double Z { get; set; }

        public double Distance
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        public double Ro => CalculateRo();

        private double CalculateRo()
        {
            if ( X > 0 && Y == 0 )  
            {
                return 0;
            }
            else if ( X == 0 && Y > 0 )
            {
                return Math.PI / 2;
            }
            else if ( X < 0 && Y == 0)
            {
                return Math.PI;
            }
            else if ( X == 0 && Y < 0)
            {
                return 3 / 2 * Math.PI;
            }
            else if (X > 0 && Y > 0)
            {
                return Math.Atan(X / Y);
            }
            else if (X < 0 && Y > 0)
            {
                return Math.Atan(X / Y) + Math.PI;
            }
            else if (X < 0 && Y < 0)
            {
                return Math.Atan(X / Y) + Math.PI;
            }
            else if (X > 0 && Y < 0)
            {
                return Math.Atan(X / Y) + 2 * Math.PI;
            }
            else
            {
                return 0;
            }
        }
    }
}
