using Sas.Body.Service.Exceptions;
using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Body.Service.Models.Domain.Orbits.Helpers;

namespace Sas.Body.Service.Models.Domain.Orbits
{
    public static class OrbitFactory
    {
        private const double TwoBodyProblemMassRatioLimit = 0.03;

        public static PositionedOrbit GetOrbit(BodyDomain body, BodyDomain other, double G)
        {
            double massRatio = body.Mass / other.Mass;
            if (massRatio <= TwoBodyProblemMassRatioLimit)
            {
                return TwoBodyProblemOrbit.GetOrbit(body, other, G);
            }
            else
            {
                // TODO: create another orbit
                throw new TwoBodyProblemAssumptionNotSatisfiedException($"Masses of the bodies do not satisy the two body assumption. Mass ratio is {massRatio}, while should be not greater than {TwoBodyProblemMassRatioLimit}");
            }
        }
    }
}
