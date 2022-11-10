using Sas.Domain.Orbits.Primitives;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using System;

namespace Sas.Domain.Orbits
{
    public class Orbit
    {
        #region fields

        private OrbitType _type;
        private double _u;
        private double _a;
        private double _e;
        private double _m;
        private double _w;
        private double _i;
        private double _omega;
        private double _phi;
        private double _ae;
        private double _period;

        #endregion

        #region properties

        /// <summary>
        /// Type of the orbit
        /// </summary>
        public OrbitType OrbitType => _type;

        /// <summary>
        /// Semi major axis
        /// </summary>
        public double SemiMajorAxis => _a;

        /// <summary>
        /// Eccentricity
        /// </summary>
        public double Eccentricity => _e;

        /// <summary>
        /// Mean anomaly
        /// </summary>
        public double MeanAnomaly => _m;

        /// <summary>
        /// Argument of periapsis
        /// </summary>
        public double ArgumentOfPeriapsis => _w;

        /// <summary>
        /// Inclination
        /// </summary>
        public double Inclination => _i;

        /// <summary>
        /// Ascending node
        /// </summary>
        public double AscendingNode => _omega;

        /// <summary>
        /// True anomaly
        /// </summary>
        public double TrueAnomaly => _phi;

        /// <summary>
        /// Eccentric Anomaly - angle that define position of the body at auxiliary circle 
        /// </summary>
        public double EccentricAnomaly => _ae;

        /// <summary>
        /// Period - time for full circulation around focus point. 
        /// Returns NaN when parabolic or hyperbolic orbit. 
        /// </summary>
        public double Period => _period;

        #endregion

        #region constructors

        /// <summary>
        /// Create Orbit from position and velocity
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="u">G(m1+m2)</param>
        public Orbit(Vector position, Vector velocity, double u)
        {
            _u = u;
            AssignFileds(position, velocity);
        }

        #endregion

        #region public method
        public double GetU() => _u;
        #endregion

        #region private methods
        private void AssignFileds(Vector position, Vector velocity)
        {
            double u = _u;
            double r = position.Magnitude;
            double v = velocity.Magnitude;
            double a = 1 / (2 / r - v * v / u);
            Vector hVector = Vector.CrossProduct(position, velocity);
            double h = hVector.Magnitude;
            Vector nVector = new(-hVector.Y, hVector.X, 0); //first node vector n
            double n = nVector.Magnitude;
            Vector eVector = 1 / u * Vector.CrossProduct(velocity, hVector) - 1 / r * position;
            double e = eVector.Magnitude;
            double phi = GetTrueAnomaly(position, velocity, r, eVector, e);
            double ae = GetEccentricAnomaly(e, phi);
            double m = GetMeanAnomaly(e, ae);

            _a = a;
            _e = eVector.Magnitude; // or Math.Sqrt(1 + v * v * h * h / (u * u) - 2 * (h * h / (u * r)));
            _type = GetOrbitType(e);
            _i = GetInclination(hVector, h); ;
            _omega = GetAscendingNode(nVector, n);
            _w = GetArgumentOfPeriapsis(eVector, e, nVector, n);
            _phi = phi;
            _ae = ae;
            _m = m;
            _period = 2 * Constants.PI * Math.Sqrt((Math.Pow(a, 3) / u));
        }

        private double GetMeanAnomaly(double e, double ae)
        {
            if (_type == OrbitType.Circular || _type == OrbitType.Elliptic)
            {
                return ae - e * Math.Sin(ae);
            }
            else if (_type == OrbitType.Parabolic || _type == OrbitType.Hyperbolic)
            {
                return e * Math.Sinh(ae) - ae;
            }
            else throw new Exception($"Cannot calculate mean anomaly. Type orbit is unknown.");
        }

        private OrbitType GetOrbitType(double e)
        {
            if (e == 0) return OrbitType.Circular;
            else if (e > 0 && _e < 1) return OrbitType.Elliptic;
            else if (e == 1) return OrbitType.Parabolic;
            else if (e > 1) return OrbitType.Hyperbolic;
            else throw new Exception($"Cannot predict orbit type. Unsupported value of eccentricity = {e}");
        }

        private double GetEccentricAnomaly(double e, double phi)
        {
            if (_type == OrbitType.Circular || _type == OrbitType.Elliptic)
            {
                double cosAE = (e + Math.Cos(phi)) / (1 + e * Math.Cos(phi));
                return Math.Acos(cosAE);
            }
            else if (_type == OrbitType.Parabolic || _type == OrbitType.Hyperbolic)
            {
                double tanGudermannianAngle = (Math.Pow(e, 2) - 1) * Math.Sin(phi) / (1 + e * Math.Cos(phi));
                double gudermannianAngle = Math.Atan(tanGudermannianAngle); // 
                return Math.Log(Math.Tan(gudermannianAngle / 2 + Math.PI / 4));
                
            }
            else throw new Exception("Cannot calculate eccentric anomaly. Type orbit is unknown.");
        }

        private double GetTrueAnomaly(Vector position, Vector velocity, double r, Vector eVector, double e)
        {
            double phi = Math.Acos(Vector.DotProduct(eVector, position) / (e * r));
            return Vector.DotProduct(position, velocity) >= 0 ? phi : 2 * Math.PI - phi;
        }

        private double GetArgumentOfPeriapsis(Vector eVector, double e, Vector nVector, double n)
        {
            return eVector.Z >= 0 ? 
                Math.Acos(Vector.DotProduct(nVector, eVector) / (n * e)) :
                2 * Math.PI - Math.Acos(Vector.DotProduct(nVector, eVector) / (n * e));
        }

        private static double GetInclination(Vector hVector, double h)
        {
            return h != 0 ? 
                Math.Acos(hVector.Z / h) :
                double.NaN;
        }
        private double GetAscendingNode(Vector nVector, double n)
        {
            if (n != 0)
            {
                if (nVector.Y >= 0) return Math.Acos(nVector.X / n);
                else return 2 * Math.PI - Math.Acos(nVector.X / n);
            }
            else
            {
                return double.NaN;
            }
        }

        #endregion
    }
}
