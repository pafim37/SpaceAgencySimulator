using System.Collections.Generic;
using Xunit;

namespace Sas.Mathematica.Tests
{
    public class MatrixTest
    {
        private readonly Matrix _matrix;
        public MatrixTest()
        {
            _matrix = new Matrix(0, 1, 2, 4, 5, 6, 7, 8, 9, 0, -1, -2, -3, -4, -5, -6);
        }
        public static IEnumerable<object[]> DimData =>
            new List<object[]> {
                new object[] { new Matrix(13), 1},
                new object[] { new Matrix(-2, 3, 1, -4), 2 },
                new object[] { new Matrix(1, 0, 3, -2, -1, 2, 4, 2, 3), 3},
                new object[] { new Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6), 4},
            };
        public static IEnumerable<object[]> DetData =>
            new List<object[]> {
                new object[] { new Matrix(13), 13.0},
                new object[] { new Matrix(-2, 3, 1, -4), 5.0 },
                new object[] { new Matrix(1, 0, 3, -2, -1, 2, 4, 2, 3), -7.0},
                new object[] { new Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6) ,0.0},
            };

        [Theory, MemberData(nameof(DetData))]
        public void Determinant_ReturnMatrixDeterminant(Matrix matrix, double det)
        {
            var result = matrix.Determinant;
            Assert.Equal(result, det);
        }

        [Theory, MemberData(nameof(DimData))]
        public void Dimension_ReturnMatrixDimension(Matrix matrix, int dim)
        {
            var result = matrix.GetDimension();
            Assert.Equal(result, dim);
        }
    }
}