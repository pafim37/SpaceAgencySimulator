using Sas.Body.Service.Extensions.PointExtensions;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Rotation;
using Sas.Mathematica.Service.Vectors;
using System.Drawing;

namespace Sas.Body.Service.Models.Domain.Orbits.Points
{
    public static class GetHyperbolicOrbitPoints
    {
        public static List<Point> GetPoints(PositionedOrbit orbit, int segments = 180)
        {
            double a = orbit.OrbitDescription!.SemiMajorAxis!.Value;
            double b = orbit.OrbitDescription!.SemiMinorAxis!.Value;
            double fi = orbit.OrbitDescription.RotationAngle;
            double inc = orbit.OrbitDescription.Inclination;
            double theta = orbit.OrbitDescription.Theata;
            Vector center = orbit.Center!;
            Vector eVector = orbit.OrbitDescription.EccentricityVector;

            List<Point> points = [];
            int halfSegments = segments / 2;
            for (int i = -halfSegments; i <= halfSegments; i++)
            {
                double t = 2 * Constants.PI * i / segments;

                // base points
                double xBase = -a * Math.Cosh(t);
                double yBase = b * Math.Sinh(t);
                double zBase = 0;

                Vector vectorToRotate = new(xBase, yBase, zBase);
                Vector rotVect = Rotation.Rotate(vectorToRotate, Vector.Oz, fi);
                Vector resVec = Rotation.Rotate(rotVect, Vector.Oy, theta);
                Vector incVec = Rotation.Rotate(resVec, eVector, inc);
                Vector finVec = -incVec + center;
                points.Add(finVec.AsPoint());
            }
            return points;
        }
    }
}
