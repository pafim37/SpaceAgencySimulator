using Sas.Orbit.Service.Orbits.OrbitalElements;
using Sas.Orbit.Service.Orbits.Primitives;

namespace Sas.Orbit.Service.Orbits.Points
{
    public static class GetOrbitPointsFactory
    {
        public static List<Point> GetPoints(IOrbitElements orbit)
        {
            if (orbit.OrbitType == OrbitType.Elliptic)
            {
                return GetEllipticOrbitPoints.GetPoints(orbit);
            }
            else if (orbit.OrbitType == OrbitType.Hyperbolic)
            {
                return GetHyperbolicOrbitPoints.GetPoints(orbit);
            }
            else
            {
                return [];
            }
        }
    }
}
