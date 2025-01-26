using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Rotation;
using Sas.Mathematica.Service.Vectors;
using System.Drawing;

namespace Sas.Body.Service.Models.Domain.Orbits.Points
{
    public static class GetHyperbolicOrbitPoints
    {
        public static List<Point> GetPoints(double semiMajorAxis, double semiMinorAxis, Vector center, double rotataionAngle, int segments = 360)
        {
            List<Point> points = [];
            int half = segments / 2;
            for (int i = -half; i <= half; i++)
            {
                double t = 2 * Constants.PI * i / segments;
                double xBase = -semiMajorAxis * Math.Cosh(t);
                double yBase = semiMinorAxis * Math.Sinh(t);
                double zBase = 0;

                // rotation z
                double xRot = xBase * Math.Cos(rotataionAngle) - yBase * Math.Sin(rotataionAngle);
                double yRot = xBase * Math.Sin(rotataionAngle) + yBase * Math.Cos(rotataionAngle);
                double zRot = zBase;

                // roation inclinaction
                double xInc = xRot;
                double yInc = yRot;
                double zInc = zRot;

                // translate
                double xResult = -xInc + center.X;
                double yResult = -yInc + center.Y;
                double zResult = -zInc + center.Z;

                points.Add(new Point(xResult, yResult, zResult)); // TODO: add inclinaction

            }
            return points;
        }
    }
}
