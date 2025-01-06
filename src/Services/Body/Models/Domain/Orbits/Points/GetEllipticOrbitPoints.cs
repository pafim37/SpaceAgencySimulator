using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits.Points
{
    public class GetEllipticOrbitPoints
    {
        public static List<Point> GetPoints(double semiMajorAxis, double semiMinorAxis, Vector center, double rotation = 0, int segments = 360)
        {
            List<Point> points = [];
            for (int i = 0; i <= segments; i++)
            {
                double t = 2 * Constants.PI * i / segments;
                double x = semiMajorAxis * Math.Cos(t) * Math.Cos(rotation) - semiMinorAxis * Math.Sin(t) * Math.Sin(rotation) + center.X;
                double y = semiMajorAxis * Math.Cos(t) * Math.Sin(rotation) + semiMinorAxis * Math.Sin(t) * Math.Cos(rotation) + center.Y;
                double z = center.Z;
                points.Add(new Point { X = x, Y = y, Z = z });
            }
            return points;
        }
    }
}
