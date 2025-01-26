using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Body.Service.Models.Domain.Orbits.Helpers;

namespace Sas.Body.Service.Models.Domain.Orbits
{
    public static class OrbitFactory
    {
        public static PositionedOrbit? GetOrbit(BodyDomain body, BodyDomain other, double G)
        {
            try
            {
                return TwoBodyProblemOrbit.Calculate(body, other, G);
            }
            catch
            {
                // TODO: create another orbit
                // TOD0:log it
                return null;
            }
        }

    }
}
