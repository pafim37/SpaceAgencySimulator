using Sas.Mathematica.Service.Matrices;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Service.Rotation
{
    public class Rotation
    {
        public static Vector Rotate(Vector vectorToRotate, Vector axis, double angle, bool inverseRotation = false)
        {
            int vectorDim = 3;
            if (vectorToRotate.Length != vectorDim)
            {
                throw new ArgumentException($"Vector to rotate has incorrect length. Expected (3), Actual {vectorToRotate.Length}");
            }
            if (axis.Length != vectorDim)
            {
                throw new ArgumentException($"Axis vector has incorrect length. Expected (3), Actual {vectorToRotate.Length}");
            }
            double[] rotationMatrixElement = CalculateElementsOfRotationMatix(axis, angle, inverseRotation);
            Matrix rotationMatrix = new(rotationMatrixElement, vectorDim, vectorDim);
            Matrix vectorToRotateAsMatrix = new(vectorToRotate.GetElements(), 3, 1);
            return (rotationMatrix * vectorToRotateAsMatrix).ToVector();
        }

        public static Matrix RotationMatrix3D(double argumentOfPeriapsis,
                                         double inclination,
                                         double ascendingNode)
        {
            double cO = Math.Cos(ascendingNode);
            double sO = Math.Sin(ascendingNode);
            double ci = Math.Cos(inclination);
            double si = Math.Sin(inclination);
            double co = Math.Cos(argumentOfPeriapsis);
            double so = Math.Sin(argumentOfPeriapsis);

            // Rz(Ω)
            Matrix RzO = new([cO, -sO, 0, sO, cO, 0, 0, 0, 1], 3, 3);

            // Rx(i)
            Matrix Rxi = new([1, 0, 0, 0, ci, -si, 0, si, ci], 3, 3);

            // Rz(ω)
            Matrix Rzo = new([co, -so, 0, so, co, 0, 0, 0, 1], 3, 3);

            return RzO * Rxi * Rzo;
        }

        private static double[] CalculateElementsOfRotationMatix(Vector rotationAxis, double angle, bool inverseRotation)
        {
            angle = inverseRotation ? -angle : angle;
            Vector axis = rotationAxis.GetNormalize();
            double ux = axis[0];
            double uy = axis[1];
            double uz = axis[2];

            double ux2 = axis[0] * axis[0];
            double uy2 = axis[1] * axis[1];
            double uz2 = axis[2] * axis[2];

            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            double cos1 = 1 - cos;
            return
            [
                cos + ux2 * cos1,
                ux * uy * cos1 - uz * sin,
                ux * uz * cos1 + uy * sin,
                uy * ux * cos1 + uz * sin,
                cos + uy2 * cos1,
                uy * uz * cos1 - ux * sin,
                uz * ux * cos1 - uy * sin,
                uz * uy * cos1 + ux * sin,
                cos + uz2 * cos1
            ];
        }
    }
}
