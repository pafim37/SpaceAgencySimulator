using Sas.Body.Service.Extensions.BodyExtensions;
using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
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
            PositionedOrbit positionedOrbit = new(body.Name, orbitDescription)
            {
                Center = GetCenter(other, orbitDescription)
            };
            return positionedOrbit;
        }

        private static Vector GetCenter(BodyDomain other, OrbitDescription orbitDescription)
        {
            if (orbitDescription.SemiMajorAxis == null)
                throw new InvalidOperationException("SemiMajorAxis cannot be null.");

            double a = orbitDescription.SemiMajorAxis.Value;
            Vector eVec = orbitDescription.EccentricityVector;

            return other.Position - a * eVec;
        }
    }
}
