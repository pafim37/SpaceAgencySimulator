using Sas.Domain.Exceptions;
using Sas.Domain.Models.Bodies;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Models.Orbits
{
    public static class OrbitFactory
    {
        public static Orbit CalculateOrbit(Vector position, Vector velocity, double u)
        {
            double e = GetEccentricity(position, velocity, u);
            if (e > 0 && e < 1) return new EllipticOrbit(position, velocity, u);
            else if (e > 1) return new HyperbolicOrbit(position, velocity, u);
            else if (e == 0) return new CircularOrbit(position, velocity, u);
            else if (e == 1) return new ParabolicOrbit(position, velocity, u);
            else throw new UnknownOrbitTypeException($"Cannot predict orbit type. Unsupported value of eccentricity = {e}");
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
    }
}
