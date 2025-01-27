using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Body.Service.Models.Domain.Orbits.Primitives;

namespace Sas.Body.Service.Models.Domain.Orbits.Points
{
    public static class GetOrbitPointsFactory
    {
        public static List<Point> GetPoints(IPositionedOrbit positionedOrbit)
        {
            IOrbitDescription? orbit = positionedOrbit.OrbitDescription;
            if (orbit.OrbitType == OrbitType.Elliptic)
            {
                return GetEllipticOrbitPoints.GetPoints(positionedOrbit);
            }
            else if (orbit.OrbitType == OrbitType.Hyperbolic)
            {
                return GetHyperbolicOrbitPoints.GetPoints(positionedOrbit);
            }
            else
            {
                return [];
            }
        }
    }
}
