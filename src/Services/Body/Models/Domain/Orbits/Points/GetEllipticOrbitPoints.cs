using Sas.Body.Service.Extensions.PointExtensions;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Rotation;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits.Points
{
    public class GetEllipticOrbitPoints
    {
        public static List<Point> GetPoints(IPositionedOrbit orbit, int segments = 360)
        {
            double a = orbit.OrbitDescription!.SemiMajorAxis!.Value;
            double b = orbit.OrbitDescription!.SemiMinorAxis!.Value;
            double fi = orbit.Phi;
            double th = orbit.Theta;
            double eta = orbit.Eta;
            Vector center = orbit.Center!;
            Vector eVector = orbit.OrbitDescription.EccentricityVector;

            List<Point> points = [];
            for (int i = 0; i <= segments; i++)
            {
                double t = 2 * Constants.PI * i / segments;

                // base points
                double xBase = a * Math.Cos(t);
                double yBase = b * Math.Sin(t);
                double zBase = 0;

                Vector vectorToRotate = new(xBase, yBase, zBase);
                Vector fiVec = Rotation.Rotate(vectorToRotate, Vector.Oz, fi);
                Vector thVec = Rotation.Rotate(fiVec, Vector.Oy, th);
                Vector etaVec = Rotation.Rotate(thVec, eVector, eta, true);
                Vector resultVec = -etaVec + center;
                points.Add(resultVec.AsPoint());
            }
            return points;
        }
    }
}
