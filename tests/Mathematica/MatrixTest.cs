using NUnit.Framework;
using Sas.Mathematica.Service.Matrices;
using System;

namespace Sas.Mathematica.Tests
{
    [TestFixture]
    public class MatrixTest
    {
        private readonly double[] _oneElement = new double[] { 1 };
        private readonly double[] _threeElements = new double[] { 1, 2, 3 };
        private readonly double[] _fourElements = new double[] { 1, 2, 3, 4 };
        private readonly double[] _sixElements = new double[] { 1, 2, 3, 4, 5, 6 };
        private readonly double[] _nineElements = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        static object[] TransposeMatrixData =
        {
            new object[] { new double[] { 1 }, 1, 1, new double[] { 1 } },
            new object[] { new double[] { 1, 2, 3, 4, 5, 6 }, 2, 3, new double[] { 1, 4, 2, 5, 3, 6} },
            new object[] { new double[] { 1, 2, 3, 4, 5, 6 }, 3, 2, new double[] { 1, 3, 5, 2, 4, 6 } },
            new object[] { new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3, 3, new double[] { 1 , 4, 7, 2, 5, 8, 3, 6, 9} }
        };
        
        static object[] InvertMatrixData =
        {
            new object[] { new double[] { 1, 1, 1, 0, 0, 1, 0, 1, 0 }, 3, 3, new double[] { 1 , -1, -1, 0, 0, 1, 0, 1, 0} }
        };

        [Test]
        public void CreateMatrixThrowsExceptionWhenElementsAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Matrix(elements: null, 2, 2));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void CreateMatrixThrowsExceptionWhenNonPositiveRows(int row)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(_oneElement, row, 2));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void CreateMatrixThrowsExceptionWhenNonPositiveColumns(int col)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(_oneElement, 2, col));
        }

        [TestCase(2, 1)]
        [TestCase(1, 2)]
        [TestCase(3, 2)]
        [TestCase(2, 3)]
        public void CreateMatrixThrowsExceptionWhenNumberOfElementsAreNotEqualToProvidedDimension(int row, int col)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(_fourElements, row, col));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(_threeElements, row, col));
        }

        [Test]
        public void CreateMatrixWhenPassingArguments()
        {
            Assert.NotNull(new Matrix(_fourElements, 2, 2 ));
            Assert.NotNull(new Matrix(_sixElements, 2, 3 ));
            Assert.NotNull(new Matrix(_sixElements, 3, 2 ));
        }

        [Test]
        public void MatrixElementsAreAccessibleByBrackets()
        {
            Matrix matrix = new Matrix(_sixElements, 2, 3);

            Assert.AreEqual(1, matrix[1, 1]);
            Assert.AreEqual(2, matrix[1, 2]);
            Assert.AreEqual(3, matrix[1, 3]);
            Assert.AreEqual(4, matrix[2, 1]);
            Assert.AreEqual(5, matrix[2, 2]);
            Assert.AreEqual(6, matrix[2, 3]);
        }
        
        [Test]
        public void GetAllElementsReturnsElements()
        {
            Matrix matrix = new Matrix(_sixElements, 2, 3);

            Assert.AreEqual(_sixElements, matrix.GetAllElements());
        }

        [TestCase(1, new double[] {1, 4, 7})]
        [TestCase(2, new double[] {2, 5, 8})]
        [TestCase(3, new double[] {3, 6, 9})]
        public void GetColumnReturnsColumn(int column, double[] elements)
        {
            Matrix matrix = new Matrix(_nineElements, 3, 3);

            Assert.AreEqual(matrix.GetColumn(column), elements);
        }

        [Test]
        public void GetDeterminantReturnsDeterminant()
        {
            Matrix matrix = new Matrix(_nineElements, 3, 3);

            double determinant = matrix.GetDeterminant();

            Assert.AreEqual(expected: 0, determinant);
        }

        [Test]
        public void GetDeterminantThrowsExceptionWhenMatrixIsNoSquare()
        {
            Matrix matrix = new Matrix(_threeElements, 1, 3);

            Assert.Throws<Exception>(() => matrix.GetDeterminant());
        }

        [Test]
        public void EqualsReturnsTrueWhenCompereSameMatrix()
        {
            Matrix matrixLeft = new Matrix(_sixElements, 2, 3);
            Matrix matrixRight = new Matrix(_sixElements, 2, 3);

            bool result = matrixLeft.Equals(matrixRight);
            Assert.True(result);
        }
        
        [Test]
        public void EqualsReturnsFalseWhenCompereDifferentMatrix()
        {
            Matrix matrixLeft = new Matrix(_sixElements, 2, 3);
            Matrix matrixRight = new Matrix(_sixElements, 3, 2);

            bool result = matrixLeft.Equals(matrixRight);
            Assert.False(result);
        }

        [Test]
        public void MatrixReturnsItsProperties()
        {
            Matrix matrix = new Matrix(_sixElements, 3, 2);

            Assert.AreEqual(3, matrix.RowsNumber);
            Assert.AreEqual(2, matrix.ColumnsNumber);
        }

        [Test]
        public void GetDimensionReturnsDimensionOfSquareMatrix()
        {
            Matrix matrix = new Matrix(_nineElements, 3, 3);

            Assert.AreEqual(3, matrix.GetDimension());
        }
        
        [Test]
        public void GetDimensionThrowsExceptionWhenMatrixIsNotSquare()
        {
            Matrix matrix = new Matrix(_sixElements, 2, 3);

            Assert.Throws<Exception>(() => matrix.GetDimension());
        }

        [TestCaseSource(nameof(TransposeMatrixData))]
        public void TransposeReturnsTransposeMatrix(double[] elements, int rows, int cols, double[] trasposeElements)
        {
            Matrix matrix = new Matrix(elements, rows, cols);

            Matrix transpose = matrix.Transpose();
            
            Assert.AreEqual(cols, transpose.RowsNumber);
            Assert.AreEqual(rows, transpose.ColumnsNumber);
            for (int i = 0; i < rows * cols; i++)
            {
                Assert.AreEqual(matrix[i], trasposeElements[i]);
            }
        }

        [TestCaseSource(nameof(InvertMatrixData))]
        public void InvertMatrixReturnsInvertedMatrix(double[] elements, int rows, int cols, double[] inverseElements)
        {
            Matrix matrix = new Matrix(elements, rows, cols);

            Matrix invert = matrix.Invert();
            for (int i = 0; i < rows * cols; i++)
            {
                Assert.AreEqual(matrix[i], inverseElements[i]);
            }
        }
    }
}
