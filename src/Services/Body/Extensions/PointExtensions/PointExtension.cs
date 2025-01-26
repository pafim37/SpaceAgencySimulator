using Sas.Body.Service.Models.Domain.Orbits.Points;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Extensions.PointExtensions
{
    public static class PointExtension
    {
        public static Vector AsVector(this Point point)
        {
            return new Vector(point.X, point.Y, point.Z);
        }  

        public static Point AsPoint (this Vector vector)
        {
            return new Point(vector.X, vector.Y, vector.Z);
        }
    }
}
