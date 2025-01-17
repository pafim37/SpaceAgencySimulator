using Sas.Body.Service.Models.Domain.Orbits.Primitives;

namespace Sas.Body.Service.Models.Domain.Orbits.Points
{
    public static class GetOrbitPointsFactory
    {
        public static List<Point>? GetPoints(Orbit orbit)
        {
            if (orbit.OrbitType == OrbitType.Elliptic)
            {
                return GetEllipticOrbitPoints.GetPoints(orbit.SemiMajorAxis!.Value, orbit.SemiMinorAxis!.Value, orbit.Center, orbit.RotationAngle);
            }
            else
            {
                return null;
            }
        }
    }
}
