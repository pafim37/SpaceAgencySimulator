using Sas.Domain.Bodies;
using Sas.Domain.Exceptions;
using Sas.Mathematica;

namespace Sas.Domain
{
    public class Orbit
    {
        /// <summary>
        /// Semi major axis
        /// </summary>
        public double SemiMajorAxis { get; private set; }

        /// <summary>
        /// Semi minor axis
        /// </summary>
        public double SemiMinorAxis { get; private set; }

        /// <summary>
        /// Angular momentum per unit mass
        /// </summary>
        public double AngularMomentumPerUnitMass { get; private set; }

        /// <summary>
        /// Semi latus rectum
        /// </summary>
        public double SemiLatusRectum { get; private set; }

        /// <summary>
        /// Eccentricity
        /// </summary>
        public double Eccentricity { get; private set; }

        /// <summary>
        /// True anomaly
        /// </summary>
        public double TrueAnomaly { get; private set; }

        /// <summary>
        /// Argument of periapsis
        /// </summary>
        public double ArgumentOfPeriApsis { get; private set; }

        /// <summary>
        /// Inclination
        /// </summary>
        public double Inclination { get; private set; }

        /// <summary>
        /// Ascending node
        /// </summary>
        public double AscendingNode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="u"></param>
        public Orbit(Vector position, Vector velocity, double u)
        {
            AssignProperties(position, velocity, u);
        }

        public Orbit(BodyBase body)
        {
            if (body is not null)
            {
                if (body.SurroundedBody is not null)
                {
                    var position = body.GetPositionRelatedToSurroundedBody();
                    var velocity = body.GetVelocityRelatedToSurroundedBody();
                    var u = Constants.G * (body.Mass + body.SurroundedBody.Mass);
                    AssignProperties(position, velocity, u);
                }
                else
                {
                    throw new SurroundedBodyException("Surrounded body is not assigned");
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(body));
            }
        }

        #region private methods
        private void AssignProperties(Vector position, Vector velocity, double u)
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
            AngularMomentumPerUnitMass = h;
            Eccentricity = e;
            ArgumentOfPeriApsis = Math.Acos(Vector.DotProduct(velocity, eVector));
        }
        #endregion
    }
}
