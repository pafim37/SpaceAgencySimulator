using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Mathematica
{
    public class ReferenceSystem
    {
        private double _xO; // x origin
        private double _yO; // y origin
        private double _zO; // z origin
        private double _x; // x point
        private double _y; // y point
        private double _z; // z point
        private double _r; // distance from origin to point
        private double _phi; // angle in x & y plane
        private double _th; // angle in x & z plane
        public double X { get => _x; private set => _x = value; }
        public double Y { get => _y; private set => _y = value; }
        public double Z { get => _z; private set => _z = value; }
        public double R { get => _r; private set => _r = value; }
        public double Phi { get => _phi; private set => _phi = value; }
        public double Th { get => _th; private set => _th = value; }

        public ReferenceSystem(Vector origin)
        {
            _xO = origin.X;
            _yO = origin.Y;
            _zO = origin.Z;
        }

        public ReferenceSystem()
        {
            _xO = 0;
            _yO = 0;
            _zO = 0;
        }

        public void Cartesian(Vector vector)
        {
            _x = vector.X - _xO;
            _y = vector.Y - _yO;
            _z = vector.Z - _zO;
            _r = GetR();
            _phi = GetPhi();
            _th = GetTh();
        }

        private double GetR()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }
        private double GetPhi()
        {
            if (X > 0 && Y > 0)
            {
                return Math.Atan(Y / X);
            }
            else if (X < 0 && Y > 0)
            {
                return Math.PI + Math.Atan(Y / X);
            }
            else if (X < 0 && Y < 0)
            {
                return Math.PI + Math.Atan(Y / X);
            }
            else if (X > 0 && Y < 0)
            {
                return 2 * Math.PI + Math.Atan(Y / X);
            }
            else if (X > 0 && Y == 0)
            {
                return 0.0;
            }
            else if (X == 0 && Y > 0)
            {
                return Math.PI / 2;
            }
            else if (X < 0 && Y == 0)
            {
                return Math.PI;
            }
            else if (X == 0 && Y < 0)
            {
                return 3 * Math.PI / 2;
            }
            else return 0.0;

        }

        private double GetTh()
        {
            if (X == 0)
            {
                return Math.Sign(Z) * 0.5 * Math.PI;
            }
            else if (Z > 0)
            {
                return Math.Sign(X) * Math.Atan(Z / X);
            }
            else if (Z < 0)
            {
                return Math.Sign(X) * Math.Atan(Z / X);
            }
            else return 0.0;
        }
    }
}
