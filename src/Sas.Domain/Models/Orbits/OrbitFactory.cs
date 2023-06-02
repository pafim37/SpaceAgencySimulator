using Sas.Domain.Exceptions;
using Sas.Domain.Models.Bodies;
using Sas.Domain.Models.Orbits.Primitives;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Matrices;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Models.Orbits
{
    public static class OrbitFactory
    {
        public static Orbit CalculateOrbit(Vector position, Vector velocity, double u)
        {
            double e = GetEccentricity(position, velocity, u);
            e = Math.Round(e, 5);
            OrbitType type = GetOrbitType(e);
            return type switch
            {
                OrbitType.Circular => new CircularOrbit(position, velocity, u),
                OrbitType.Elliptic => new EllipticOrbit(position, velocity, u),
                OrbitType.Parabolic => new ParabolicOrbit(position, velocity, u),
                OrbitType.Hyperbolic => new HyperbolicOrbit(position, velocity, u),
                _ => throw new UnknownOrbitTypeException($"Cannot create orbit. Unknown orbit type {type}")
            };
        }
        public static Orbit CalculateOrbit(Body body, double u)
        {
            return CalculateOrbit(body.Position, body.Velocity, u);
        }

        public static Orbit CalculateOrbit(Body smallBody, Body massiveBody, double G = Constants.G)
        {
            double u = G * (smallBody.Mass + massiveBody.Mass);
            return CalculateOrbit(smallBody.Position, smallBody.Velocity, u);
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
