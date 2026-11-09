using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Exceptions;

namespace Sas.Orbit.Service.Orbits.OrbitalElements
{
    public static class OrbitElementsFactory
    {
        /// <summary>
        /// Gets the orbit elements based on the position, velocity, and gravitational parameter
        /// </summary>
        /// <param name="positionRel">Position related to the center of the gravitational field</param>
        /// <param name="velocityRel">Velocity related to the center of the gravitational field</param>
        /// <param name="gravitationalParameter">The gravitational parameter of the central body G(M+m)</param>
        /// <returns>Orbit elements</returns>
        /// <exception cref="UnknownOrbitTypeException"></exception>
        public static OrbitElements GetOrbitElements(Vector positionRel, Vector velocityRel, double gravitationalParameter)
        {
            double e = GetEccentricity(positionRel, velocityRel, gravitationalParameter);
            if (e > 0 && e < 1)
            {
                return new EllipticOrbitElements(positionRel, velocityRel, gravitationalParameter);
            }
            else if (e > 1)
            {
                return new HyperbolicOrbitElements(positionRel, velocityRel, gravitationalParameter);
            }
            else if (e == 0)
            {
                return new CircularOrbitElements(positionRel, velocityRel, gravitationalParameter);
            }
            else if (e == 1)
            {
                return new ParabolicOrbitElements(positionRel, velocityRel, gravitationalParameter);
            }
            else
            {
                throw new UnknownOrbitTypeException($"Cannot predict orbit type. Unsupported value of eccentricity = {e}");
            }
        }

        private static double GetEccentricity(Vector position, Vector velocity, double gravitationalParameter)
        {
            double r = position.Magnitude;
            Vector hVector = Vector.CrossProduct(position, velocity);
            Vector eVector = 1 / gravitationalParameter * Vector.CrossProduct(velocity, hVector) - 1 / r * position;
            double e = eVector.Magnitude;
            return e;
        }
    }
}
