using Sas.Mathematica.Service.Rotation;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.OrbitalElements;

namespace Sas.Orbit.Service.Orbits.Points
{
    public class GetEllipticOrbitPoints
    {
        public static List<Point> GetPoints(IOrbitElements orbit, int segments = 50)
        {
            List<Point> points = [];

            double a = orbit.SemiMajorAxis ?? 0;
            double b = orbit.SemiMinorAxis ?? 0;

            double w = double.IsNaN(orbit.ArgumentOfPeriapsis) ? 0.0 : orbit.ArgumentOfPeriapsis;
            double i = double.IsNaN(orbit.Inclination) ? 0.0 : orbit.Inclination;
            double om = double.IsNaN(orbit.AscendingNode) ? 0.0 : orbit.AscendingNode;
            var rotation = Rotation.RotationMatrix3D(w, i, om);

            for (int k = 0; k <= segments; k++)
            {
                double t = 2 * Math.PI * k / segments;
                Vector p = new(a * Math.Cos(t), b * Math.Sin(t), 0);

                p = rotation * p;

                points.Add(new Point(p[0], p[1], p[2]));
            }
            return points;
        }
    }
}
