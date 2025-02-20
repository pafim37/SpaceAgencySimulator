using Sas.Body.Service.Extensions.BodyExtensions;
using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Body.Service.Models.Domain.Orbits.Primitives;
using Sas.Domain.Exceptions;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits.Helpers
{
    public class TwoBodyProblemOrbit
    {

        public static PositionedOrbit GetOrbit(BodyDomain body, BodyDomain other, double G)
        {
            Vector position = body.GetPositionRelatedTo(other);
            Vector velocity = body.GetVelocityRelatedTo(other);
            double u = G * (body.Mass + other.Mass);

            OrbitDescription orbitDescription = OrbitDescriptionFactory.GetOrbitDescription(position, velocity, u);
            PositionedOrbit positionedOrbit = new(body.Name, velocity, orbitDescription)
            {
                Center = GetCenter(other, orbitDescription)
            };
            return positionedOrbit;
        }

        private static Vector GetCenter(BodyDomain other, OrbitDescription orbitDescription)
        {
            switch (orbitDescription.OrbitType)
            {
                case OrbitType.Elliptic:
                    {
                        if (orbitDescription.SemiMajorAxis == null)
                        {
                            throw new InvalidOperationException("SemiMajorAxis cannot be null.");
                        }
                        double ae = orbitDescription.Eccentricity * orbitDescription.SemiMajorAxis.Value;
                        return -ae * orbitDescription.EccentricityVector.GetNormalize() + other.Position;
                    }

                case OrbitType.Hyperbolic:
                    {
                        if (orbitDescription.SemiMajorAxis == null)
                        {
                            throw new InvalidOperationException("SemiMajorAxis cannot be null.");
                        }
                        double ae = orbitDescription.Eccentricity * orbitDescription.SemiMajorAxis.Value;
                        return -ae * orbitDescription.EccentricityVector.GetNormalize() + other.Position;
                    }

                case OrbitType.Circular:
                    return new Vector(other.Position.X, other.Position.Y, 0);
                case OrbitType.Parabolic:
                    return new Vector(other.Position.Y, other.Position.X, 0);
                default:
                    throw new UnknownOrbitTypeException("Unknown orbit type while determine a center of the orbit.");
            }
        }
    }
}
