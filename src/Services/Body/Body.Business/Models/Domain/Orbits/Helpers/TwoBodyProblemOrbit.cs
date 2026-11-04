using Sas.Body.Business.Extensions.BodyExtensions;
using Sas.Body.Business.Models.Domain.Bodies;
using Sas.Body.Business.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Business.Models.Domain.Orbits.Helpers
{
    public class TwoBodyProblemOrbit
    {

        public static PositionedOrbit GetOrbit(BodyDomain body, BodyDomain other, double G)
        {
            Vector position = body.GetPositionRelatedTo(other);
            Vector velocity = body.GetVelocityRelatedTo(other);
            double u = G * (body.Mass + other.Mass);

            OrbitDescription orbitDescription = OrbitDescriptionFactory.GetOrbitDescription(position, velocity, u);
            PositionedOrbit positionedOrbit = new(body.Name, other, orbitDescription);
            return positionedOrbit;
        }
    }
}
