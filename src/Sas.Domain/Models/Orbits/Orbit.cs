﻿using Sas.Domain.Models.Orbits.Primitives;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Models.Orbits
{
    public abstract class Orbit
    {
        #region fields
        protected OrbitType _type; // orbit type
        protected double _u;       // G(m1+m2)
        protected double _a;       // semi-major axis
        protected double _b;       // semi-minor axis
        protected double _p;       // semi-latus rectum
        protected double _e;       // eccentricity
        protected double _w;       // argument of periapsis
        protected double _i;       // inclination
        protected double _omega;   // ascending node
        protected double _phi;     // true anomaly
        protected double _ae;      // eccentric anomaly
        protected double _m;       // mass 
        protected double _period;  // period
        protected double _radius;  // radius

        #endregion

        #region properties

        /// <summary>
        /// Type of the orbit
        /// </summary>
        public OrbitType OrbitType => _type;

        /// <summary>
        /// Semi major axis
        /// </summary>
        public double? SemiMajorAxis => GetSemiMajorAxis();

        /// <summary>
        /// Semi minor axis
        /// </summary>
        public double? SemiMinorAxis => GetSemiMinorAxis();

        /// <summary>
        /// Eccentricity
        /// </summary>
        public double Eccentricity => _e;

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
        /// Mean anomaly
        /// </summary>
        public double MeanAnomaly => _m;

        /// <summary>
        /// Period - time for full circulation around focus point. 
        /// Returns NaN when parabolic or hyperbolic orbit. 
        /// </summary>
        public double? Period => GetPeriod();

        /// <summary>
        /// Radius of the circular orbit 
        /// </summary>
        public double? Radius => GetRadius();

        #endregion

        #region constructors

        /// <summary>
        /// Create Orbit from position and velocity
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="u">Standard gravitational parameter: G(m1+m2)</param>
        public Orbit(Vector position, Vector velocity, double u)
        {
            _u = u;
            AssignFileds(position, velocity);
        }

        #endregion

        #region public method
        public double GetU() => _u;

        public void UpdateOrbit(Vector position, Vector velocity)
        {
            AssignFileds(position, velocity);
        }

        public void UpdateOrbit(Vector position, Vector velocity, double u)
        {
            _u = u;
            AssignFileds(position, velocity);
        }

        public abstract double? GetRadius();
        public abstract double? GetPeriod();
        public abstract double? GetSemiMajorAxis();
        public abstract double? GetSemiMinorAxis();
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
            double b = a * Math.Sqrt(1 - e * e);
            double phi = GetTrueAnomaly(position, velocity, r, eVector, e);
            double ae = GetEccentricAnomaly(e, phi);
            double m = GetMeanAnomaly(e, ae);
            double p = h * h / u;
            _a = a;
            _b = b;
            _e = eVector.Magnitude; // or Math.Sqrt(1 + v * v * h * h / (u * u) - 2 * (h * h / (u * r)));
            _i = GetInclination(hVector, h); ;
            _omega = GetAscendingNode(nVector, n);
            _w = GetArgumentOfPeriapsis(eVector, e, nVector, n);
            _phi = phi;
            _ae = ae;
            _m = m;
            _p = p;
            _period = 2 * Constants.PI * Math.Sqrt(Math.Pow(a, 3) / u);
            _radius = r;
        }

        protected abstract double GetMeanAnomaly(double e, double ae);
        protected abstract double GetEccentricAnomaly(double e, double phi);

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
