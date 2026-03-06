using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Mathematica.Service.Rotation;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits.Points
{
    public static class GetHyperbolicOrbitPoints
    {
        public static List<Point> GetPoints(IPositionedOrbit orbit, int segments = 180)
        {
            List<Point> points = [];
            IOrbitDescription desc = orbit.OrbitDescription;

            double a = desc.SemiMajorAxis ?? 0;
            double b = desc.SemiMinorAxis ?? 0;

            double w = double.IsNaN(desc.ArgumentOfPeriapsis) ? 0.0 : desc.ArgumentOfPeriapsis;
            double i = double.IsNaN(desc.Inclination) ? 0.0 : desc.Inclination;
            double om = double.IsNaN(desc.AscendingNode) ? 0.0 : desc.AscendingNode;
            int halfSegments = segments / 2;
            for (int k = -halfSegments; k <= halfSegments; k++)
            {
                double t = 2 * Math.PI * k / segments;
                Vector p = new(a * Math.Cosh(t), b * Math.Sinh(t), 0);

                p = Rotation.Rotate(p, Vector.Oz, w);
                p = Rotation.Rotate(p, Vector.Ox, i);
                p = Rotation.Rotate(p, Vector.Oz, om);

                points.Add(new Point(p[0], p[1], p[2]));
            }
            return points;
        }
    }
}
