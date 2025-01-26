using Sas.Body.Service.Models.Domain.Orbits.OrbitInfos;
using Sas.Body.Service.Models.Domain.Orbits.Primitives;

namespace Sas.Body.Service.Models.Domain.Orbits.Points
{
    public static class GetOrbitPointsFactory
    {
        public static List<Point> GetPoints(PositionedOrbit positionedOrbit)
        {
            OrbitDescription? orbit = positionedOrbit.OrbitDescription;
            if (orbit.OrbitType == OrbitType.Elliptic)
            {
                return GetEllipticOrbitPoints.GetPoints(positionedOrbit);
            }
            else if (orbit.OrbitType == OrbitType.Hyperbolic)
            {
                return GetHyperbolicOrbitPoints.GetPoints(orbit.SemiMajorAxis!.Value, orbit.SemiMinorAxis!.Value, positionedOrbit.Center!, orbit.RotationAngle);
            }
            else
            {
                return [];
            }
        }
    }
}
