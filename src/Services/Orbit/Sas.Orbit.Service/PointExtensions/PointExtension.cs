using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.Points;

namespace Sas.Orbit.Service.PointExtensions
{
    public static class PointExtension
    {
        public static Vector AsVector(this Point point)
        {
            return new Vector(point.X, point.Y, point.Z);
        }

        public static Point AsPoint(this Vector vector)
        {
            return new Point(vector.X, vector.Y, vector.Z);
        }
    }
}
