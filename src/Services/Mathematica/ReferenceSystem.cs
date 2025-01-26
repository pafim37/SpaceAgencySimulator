using Sas.Mathematica.Service.Converters;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Service
{
    /// <summary>
    /// Provides cartesian and spherical components for point in space with given origin
    /// </summary>
    public class ReferenceSystem
    {
        private double _x;           // x point
        private double _y;           // y point
        private double _z;           // z point
        private double _r;           // distance from origin to point
        private double _phi;         // angle in x & y plane
        private double _th;          // angle in x & z plane

        /// <summary>
        /// Position X (cartesian coordinate system) of point relative to the origin
        /// </summary>
        public double X { get => _x; private set => _x = value; }

        /// <summary>
        /// Position Y (cartesian coordinate system) of point relative to the origin
        /// </summary>
        public double Y { get => _y; private set => _y = value; }

        /// <summary>
        /// Position Z (cartesian coordinate system) of point relative to the origin
        /// </summary>
        public double Z { get => _z; private set => _z = value; }

        /// <summary>
        /// Position R (spherical coordinate system) of point relative to the origin
        /// </summary>
        public double R { get => _r; private set => _r = value; }

        /// <summary>
        /// Azimuthal angle Phi (spherical coordinate system) of point relative to the origin.
        /// The angle is expressed in radians.
        /// </summary>
        public double Phi { get => _phi; private set => _phi = value; }

        /// <summary>
        /// Polar angle (spherical coordinate system) of point relative to the origin.
        /// The angle is expressed in radians
        /// </summary>
        public double Th { get => _th; private set => _th = value; }

        /// <summary>
        /// Azimuthal angle Phi (spherical coordinate system) of point relative to the origin.
        /// The angle is expressed in degrees.
        /// </summary>
        public double PhiAsDeg { get => ConvertAngle.RadToDeg(_phi); }

        /// <summary>
        /// Polar angle (spherical coordinate system) of point relative to the origin.
        /// The angle is expressed in degrees
        /// </summary>
        public double ThAsDeg { get => ConvertAngle.RadToDeg(_th); }

        /// <summary>
        /// Gets vector which points to the point
        /// </summary>
        /// <returns></returns>
        public Vector GetVector() => new Vector(_x, _y, _z);

        /// <summary>
        /// Gets normalized vector which points to the point
        /// </summary>
        /// <returns></returns>
        public Vector GetNormalizedVector() => GetVector().Normalize();

        /// <summary>
        /// Creates the reference system for the point with given origin 
        /// </summary>
        /// <param name="origin"></param>
        public ReferenceSystem(Vector origin, Vector point)
        {
            AssignFields(origin, point);
        }

        /// <summary>
        /// Creates the reference system for the point and set origin at zeros
        /// </summary>
        /// <param name="origin"></param>
        public ReferenceSystem(Vector point)
        {
            Vector origin = Vector.Zero;
            AssignFields(origin, point);
        }

        private void AssignFields(Vector origin, Vector point)
        {
            _x = point.X - origin.X;
            _y = point.Y - origin.Y;
            _z = point.Z - origin.Z;
            _r = CalculateR();
            _phi = CalculatePhi();
            _th = CalculateTh();
        }

        private double CalculateR()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        private double CalculatePhi()
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

        private double CalculateTh()
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
