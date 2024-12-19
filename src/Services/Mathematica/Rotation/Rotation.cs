using Sas.Mathematica.Service.Matrices;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Service.Rotation
{
    public class Rotation
    {
        public static Vector Rotate(Vector vectorToRotate, Vector axis, double angle)
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
            double[] rotationMatrixElement = CalculateElementsOfRotationMatix(axis, angle);
            Matrix rotationMatrix = new(rotationMatrixElement, vectorDim, vectorDim);
            Matrix vectorToRotateAsMatrix = new(vectorToRotate.GetElements(), 3, 1);
            return (rotationMatrix * vectorToRotateAsMatrix).ToVector();
        }

        private static double[] CalculateElementsOfRotationMatix(Vector axis, double angle)
        {
            axis.Normalize();
            double ux = axis[0];
            double ux2 = axis[0] * axis[0];
            double uy = axis[1];
            double uy2 = axis[1] * axis[1];
            double uz = axis[2];
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
                uz * ux * cos1 + uy * sin,
                uz * uy * cos1 + ux * sin,
                cos + uz2 * cos1
            ];
        }
    }
}
