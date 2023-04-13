using Sas.Domain.Exceptions;
using Sas.Domain.Models.Orbits.Primitives;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Models.Orbits
{
    public static class OrbitFactory
    {
        public static Orbit CalculateOrbit(Vector position, Vector velocity, double u)
        {
            double e = GetEccentricity(position, velocity, u);
            var type = GetOrbitType(e);
            return type switch
            {
                OrbitType.Circular => new CircularOrbit(position, velocity, u),
                OrbitType.Elliptic => new EllipticOrbit(position, velocity, u),
                OrbitType.Parabolic => new ParabolicOrbit(position, velocity, u),
                OrbitType.Hyperbolic => new HyperbolicOrbit(position, velocity, u),
                _ => throw new UnknownOrbitTypeException($"Cannot create orbit. Unknown orbit type {type}")
            };
        }

        private static double GetEccentricity(Vector position, Vector velocity, double u)
        {
            double r = position.Magnitude;
            Vector hVector = Vector.CrossProduct(position, velocity);
            Vector eVector = 1 / u * Vector.CrossProduct(velocity, hVector) - 1 / r * position;
            double e = eVector.Magnitude;
            return e;
        }

        private static OrbitType GetOrbitType(double e)
        {
            if (e == 0) return OrbitType.Circular;
            else if (e > 0 && e < 1) return OrbitType.Elliptic;
            else if (e == 1) return OrbitType.Parabolic;
            else if (e > 1) return OrbitType.Hyperbolic;
            else throw new Exception($"Cannot predict orbit type. Unsupported value of eccentricity = {e}");
        }
    }
}
