using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Mathematica.Service.Rotation;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits.Points
{
    public class GetEllipticOrbitPoints
    {
        public static List<Point> GetPoints(IPositionedOrbit orbit, int segments = 360)
        {
            List<Point> points = [];
            IOrbitDescription desc = orbit.OrbitDescription;

            double a = desc.SemiMajorAxis ?? 0;
            double b = desc.SemiMinorAxis ?? 0;

            double w = double.IsNaN(desc.ArgumentOfPeriapsis) ? 0.0 : desc.ArgumentOfPeriapsis;
            double i = double.IsNaN(desc.Inclination) ? 0.0 : desc.Inclination;
            double om = double.IsNaN(desc.AscendingNode) ? 0.0 : desc.AscendingNode;

            for (int k = 0; k <= segments; k++)
            {
                double t = 2 * Math.PI * k / segments;
                Vector p = new(a * Math.Cos(t), b * Math.Sin(t), 0);

                p = Rotation.Rotate(p, Vector.Oz, w);
                p = Rotation.Rotate(p, Vector.Ox, i);
                p = Rotation.Rotate(p, Vector.Oz, om);

                points.Add(new Point(p[0], p[1], p[2]));
            }
            return points;
        }
    }
}
