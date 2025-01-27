using Sas.Domain.Exceptions;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions
{
    public static class OrbitInfoDescription
    {
        public static OrbitDescription CalculateOrbit(Vector position, Vector velocity, double u)
        {
            double e = GetEccentricity(position, velocity, u);
            if (e > 0 && e < 1) return new EllipticOrbitDescription(position, velocity, u);
            else if (e > 1) return new HyperbolicOrbitDescription(position, velocity, u);
            else if (e == 0) return new CircularOrbitDescription(position, velocity, u);
            else if (e == 1) return new ParabolicOrbitDescription(position, velocity, u);
            else throw new UnknownOrbitTypeException($"Cannot predict orbit type. Unsupported value of eccentricity = {e}");
        }

        private static double GetEccentricity(Vector position, Vector velocity, double u)
        {
            double r = position.Magnitude;
            Vector hVector = Vector.CrossProduct(position, velocity);
            Vector eVector = 1 / u * Vector.CrossProduct(velocity, hVector) - 1 / r * position;
            double e = eVector.Magnitude;
            return e;
        }
    }
}
