using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sas.Common;
namespace Sas.Mathematica.Tests
{
    [TestFixture]
    public class MatrixTest
    {
        [Test]
        public void CreateMatrixThrowsExceptionWhenElementsAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Matrix(2, 2, elements: null));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void CreateMatrixThrowsExceptionWhenNonPositiveRows(int row)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(row, 2, 1));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void CreateMatrixThrowsExceptionWhenNonPositiveColumns(int col)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(2, col, 1));
        }

        [TestCase(2, 1)]
        [TestCase(1, 2)]
        [TestCase(3, 2)]
        [TestCase(2, 3)]
        public void CreateMatrixThrowsExceptionWhenNumberOfElementsAreNotEqualToProvidedDimension(int row, int col)
        {
            double[] elements = new double[4];
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(row, col, elements));
        }

        [TestCase(2, 2, new double[] { 0, 0, 0, 0 })]
        [TestCase(2, 3, new double[] { 0, 0, 0, 0, 0, 0 })]
        [TestCase(3, 2, new double[] { 0, 0, 0, 0, 0, 0 })]
        public void CreateMatrixWhenPassingArguments(int row, int col, double[] elements)
        {
            Matrix matrix = new Matrix(row, col, elements);
            Assert.NotNull(matrix);
        }

        [Test]
        public void MatrixElementsAreAccessibleByBrackets()
        {
            Matrix matrix = new Matrix(3, 3, 1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual(6, matrix[2, 3]);
        }

        [TestCase(1, new double[] {1, 4, 7})]
        [TestCase(2, new double[] {2, 5, 8})]
        [TestCase(3, new double[] {3, 6, 9})]
        public void GetColumnReturnsColumn(int column, double[] elements)
        {
            Matrix matrix = new Matrix(3, 3, 1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual(matrix.GetColumn(column), elements);
        }

        [Test]
        public void GetDeterminantReturnsDeterminant()
        {
            Matrix matrix = new Matrix(3, 3, 1, 2, -1, 3, 0, 3, 2, -1, 3);

            double determinant = matrix.GetDeterminant();

            Assert.AreEqual(expected: 0, determinant);
        }

        [Test]
        public void GetDeterminantThrowsExceptionWhenMatrixIsNoSquare()
        {
            Matrix matrix = new Matrix(2, 3, 1, 2, -1, 3, 0, 3);

            Assert.Throws<Exception>(() => matrix.GetDeterminant());
        }

        [Test]
        public void EqualsProperlyCompereMatrix()
        {
            Matrix matrixLeft = new Matrix(2, 3, 1, 2, -1, 3, -1, 3);
            Matrix matrixRight = new Matrix(2, 3, 1, 2, -1, 3, -1, 3);

            bool result = matrixLeft.Equals(matrixRight);
            Assert.True(result);
        }
        
        [Test]
        public void EqualsProperlyCompereMatrix2()
        {
            Matrix matrixLeft = new Matrix(3, 2, 1, 2, -1, 3, -1, 3);
            Matrix matrixRight = new Matrix(2, 3, 1, 2, -1, 3, -1, 3);

            bool result = matrixLeft.Equals(matrixRight);
            Assert.False(result);
        }
    }
}
