using Sas.Mathematica.Service.Matrices;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Service.Extensions
{
    public static class MatrixExtension
    {
        public static Vector ToVector(this Matrix matrix)
        {
            return matrix.ToVector();
        }
    }
}
