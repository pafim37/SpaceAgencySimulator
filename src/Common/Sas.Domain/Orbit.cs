using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Domain
{
    public class Orbit
    {
        /// <summary>
        /// Semi major axis
        /// </summary>
        public double SemiMajorAxis { get; }

        /// <summary>
        /// Semi minor axis
        /// </summary>
        public double SemiMinorAxis { get; }

        /// <summary>
        /// Distance from focus
        /// </summary>
        public double DistanceFromFocus { get; }

        /// <summary>
        /// Velocity on the orbit
        /// </summary>
        public double Velocity { get; }

        /// <summary>
        /// Angular momentum per unit mass
        /// </summary>
        public double AngularMomentumPerUnitMass { get; }

        /// <summary>
        /// Semi latus rectum
        /// </summary>
        public double SemiLatusRectum { get; }

        /// <summary>
        /// Eccentricity
        /// </summary>
        public double Eccentricity { get; }

        /// <summary>
        /// G * (M + m)
        /// </summary>
        public double U { get; }

        /// <summary>
        /// True anomaly
        /// </summary>
        public double TrueAnomaly { get; }

        /// <summary>
        /// Argument of periapsis
        /// </summary>
        public double ArgumentOfPeriApsis { get; }

        /// <summary>
        /// Inclination
        /// </summary>
        public double Inclination { get; }

        /// <summary>
        /// Ascending node
        /// </summary>
        public double AscendingNode { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="u"></param>
        public Orbit(Vector position, Vector velocity, double u)
        {
            double r = position.Magnitude();
            double v = velocity.Magnitude();
            Vector hVector = Vector.CrossProduct(position, velocity);
            Vector eVector = 1 / u * Vector.CrossProduct(velocity, hVector) - r * position;
            double h = hVector.Magnitude();
            double e = Math.Sqrt(1 + v * v * h * h / (u * u) - 2 * (h * h / (u * r)));

            double _p = h * h / u;

            SemiLatusRectum = _p;
            SemiMajorAxis = 1 / (2 / r - v * v / u);
            SemiMinorAxis = _p / Math.Sqrt(1 - e * e);

        }
    }
}
