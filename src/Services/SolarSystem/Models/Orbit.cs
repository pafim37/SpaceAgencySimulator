using Sas.Mathematica;
using Sas.Mathematica.Service.Vectors;
using Sas.SolarSystem.Service.Exceptions;

namespace Sas.SolarSystem.Service.Models
{
    public class Orbit
    {

        #region properties

        /// <summary>
        /// Semi major axis
        /// </summary>
        public double SemiMajorAxis { get; private set; }

        /// <summary>
        /// Eccentricity
        /// </summary>
        public double Eccentricity { get; private set; }

        /// <summary>
        /// Mean anomaly
        /// </summary>
        public double MeanAnomaly { get; private set; }

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
        /// True anomaly
        /// </summary>
        public double TrueAnomalyAtEpoch { get; private set; }

        #endregion

        #region constructors

        /// <summary>
        /// Create Orbit from position and velocity
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="u"></param>
        public Orbit(Vector position, Vector velocity, double u)
        {
            CalculateOrbitalElements(position, velocity, u);
        }

        /// <summary>
        /// Create Orbit from another Body 
        /// </summary>
        /// <param name="body"></param>
        /// <exception cref="SurroundedBodyException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Orbit(Body body)
        {
            if (body is not null)
            {
                if (body.SurroundedBody is not null)
                {
                    var position = body.GetPositionRelatedToSurroundedBody();
                    var velocity = body.GetVelocityRelatedToSurroundedBody();
                    var u = Constants.G * (body.Mass + body.SurroundedBody.Mass);
                    CalculateOrbitalElements(position, velocity, u);
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

        #endregion

        #region private methods
        private void CalculateOrbitalElements(Vector position, Vector velocity, double u)
        {
            double r = position.Magnitude;
            double v = velocity.Magnitude;
            Vector hVector = Vector.CrossProduct(position, velocity);
            Vector eVector = 1 / u * Vector.CrossProduct(velocity, hVector) - 1 / r * position;
            double h = hVector.Magnitude;
            double e = Math.Sqrt(1 + v * v * h * h / (u * u) - 2 * (h * h / (u * r)));

            Vector nVector = new Vector(-hVector.Y, hVector.X, 0); //first node vector n
            double n = nVector.Magnitude;

            double i;
            if (h == 0) i = -9999;
            else i = Math.Acos(hVector.Z / h);

            Inclination = i;


            double p = h * h / u;

            if (n != 0)
            {
                if (nVector.Y >= 0) AscendingNode = Math.Acos(nVector.X / n);
                else AscendingNode = 2 * Math.PI - Math.Acos(nVector.X / n);
            }
            else
            {
                AscendingNode = -9999;
            }

            if (n != 0)
            {
                if (eVector.Z >= 0) ArgumentOfPeriApsis = Math.Acos(Vector.DotProduct(nVector, eVector) / (n * e));
                else ArgumentOfPeriApsis = 2 * Math.PI - Math.Acos(Vector.DotProduct(nVector, eVector) / (n * e));
            }
            else
            {
                ArgumentOfPeriApsis = -9999;
            }
            SemiMajorAxis = 1 / (2 / r - v * v / u);
            Eccentricity = e;

            if (Vector.DotProduct(position, velocity) >= 0)
            {
                TrueAnomalyAtEpoch = Math.Acos(Vector.DotProduct(eVector, position) / (e * r));
            }
            else
            {
                TrueAnomalyAtEpoch = 2 * Math.PI - Math.Acos(Vector.DotProduct(eVector, position) / (e * r));
            }
        }

        #endregion
    }
}
