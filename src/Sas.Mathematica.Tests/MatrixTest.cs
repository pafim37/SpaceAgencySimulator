using FluentAssertions;
using Sas.Mathematica.Service.Matrices;

namespace Sas.Mathematica.Tests
{

    public class MatrixTest
    {
        private readonly double[] _oneElement = [1];
        private readonly double[] _threeElements = [1, 2, 3];
        private readonly double[] _fourElements = [1, 2, 3, 4];
        private readonly double[] _sixElements = [1, 2, 3, 4, 5, 6];
        private readonly double[] _nineElements = [1, 2, 3, 4, 5, 6, 7, 8, 9];

        [Fact]
        public void CreateMatrixThrowsExceptionWhenElementsAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Matrix(elements: null, 2, 2));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void CreateMatrixThrowsExceptionWhenNonPositiveRows(int row)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(_oneElement, row, 2));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void CreateMatrixThrowsExceptionWhenNonPositiveColumns(int col)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(_oneElement, 2, col));
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(1, 2)]
        [InlineData(3, 2)]
        [InlineData(2, 3)]
        public void CreateMatrixThrowsExceptionWhenNumberOfElementsAreNotEqualToProvidedDimension(int row, int col)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(_fourElements, row, col));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(_threeElements, row, col));
        }

        [Fact]
        public void CreateMatrixWhenPassingArguments()
        {
            Assert.NotNull(new Matrix(_fourElements, 2, 2));
            Assert.NotNull(new Matrix(_sixElements, 2, 3));
            Assert.NotNull(new Matrix(_sixElements, 3, 2));
        }

        [Fact]
        public void MatrixElementsAreAccessibleByBrackets()
        {
            Matrix matrix = new(_sixElements, 2, 3);
            matrix[1, 1].Should().Be(1);
            matrix[1, 2].Should().Be(2);
            matrix[1, 3].Should().Be(3);
            matrix[2, 1].Should().Be(4);
            matrix[2, 2].Should().Be(5);
            matrix[2, 3].Should().Be(6);
        }

        [Fact]
        public void GetAllElementsReturnsElements()
        {
            Matrix matrix = new(_sixElements, 2, 3);
            matrix.GetAllElements().SequenceEqual(_sixElements);
        }

        [Theory]
        [InlineData(1, new double[] { 1, 4, 7 })]
        [InlineData(2, new double[] { 2, 5, 8 })]
        [InlineData(3, new double[] { 3, 6, 9 })]
        public void GetColumnReturnsColumn(int column, double[] elements)
        {
            Matrix matrix = new Matrix(_nineElements, 3, 3);
            matrix.GetColumn(column).Should().BeEquivalentTo(elements);
        }

        [Fact]
        public void GetIndexElementThrowsExceptionWhenIndexOutOfRange()
        {
            Matrix matrix = new([1], 1, 1);
            Action comparison = () => { double elem = matrix[2, 2]; };
            comparison.Should().Throw<IndexOutOfRangeException>();
        }

        [Fact]
        public void SetIndexElementThrwosExceptionWhenIndexOutOfRange()
        {
            Matrix matrix = new([1], 1, 1);
            Action comparison = () => { matrix[2, 2] = 2; };
            comparison.Should().Throw<IndexOutOfRangeException>();
        }


        [Fact]
        public void GetDeterminantReturnsDeterminant()
        {
            Matrix matrix = new Matrix(_nineElements, 3, 3);

            double determinant = matrix.GetDeterminant();
            matrix.GetDeterminant().Should().Be(0);
        }

        [Fact]
        public void GetDeterminantThrowsExceptionWhenMatrixIsNoSquare()
        {
            Matrix matrix = new Matrix(_threeElements, 1, 3);

            Assert.Throws<Exception>(() => matrix.GetDeterminant());
        }

        [Fact]
        public void EqualsReturnsTrueWhenCompereSameMatrix()
        {
            Matrix matrixLeft = new Matrix(_sixElements, 2, 3);
            Matrix matrixRight = new Matrix(_sixElements, 2, 3);

            bool result = matrixLeft.Equals(matrixRight);
            Assert.True(result);
        }

        [Fact]
        public void EqualsReturnsFalseWhenCompereDifferentMatrix()
        {
            Matrix matrixLeft = new Matrix(_sixElements, 2, 3);
            Matrix matrixRight = new Matrix(_sixElements, 3, 2);

            bool result = matrixLeft.Equals(matrixRight);
            Assert.False(result);
        }

        //[Fact]
        //public void MatrixReturnsItsProperties()
        //{
        //    Matrix matrix = new Matrix(_sixElements, 3, 2);

        //    Assert.AreEqual(3, matrix.RowsNumber);
        //    Assert.AreEqual(2, matrix.ColumnsNumber);
        //}

        [Fact]
        public void GetDimensionReturnsDimensionOfSquareMatrix()
        {
            Matrix matrix = new Matrix(_nineElements, 3, 3);
            matrix.GetDimension().Should().Be(3);
        }

        [Fact]
        public void GetDimensionThrowsExceptionWhenMatrixIsNotSquare()
        {
            Matrix matrix = new Matrix(_sixElements, 2, 3);

            Assert.Throws<Exception>(() => matrix.GetDimension());
        }

        [Theory]
        [InlineData(new double[] { 1 }, 1, 1, new double[] { 1 })]
        [InlineData(new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3, 3, new double[] { 1, 4, 7, 2, 5, 8, 3, 6, 9 } )]
        [InlineData(new double[] { 1, 2, 3, 4, 5, 6 }, 2, 3, new double[] { 1, 4, 2, 5, 3, 6 })]
        [InlineData(new double[] { 1, 2, 3, 4, 5, 6 }, 3, 2, new double[] { 1, 3, 5, 2, 4, 6 })]
        public void TransposeReturnsTransposeMatrix(double[] elements, int rows, int cols, double[] trasposeElements)
        {
            Matrix matrix = new(elements, rows, cols);
            Matrix transpose = matrix.Transpose();

            transpose.RowsNumber.Should().Be(cols);
            transpose.ColumnsNumber.Should().Be(rows);
            for (int i = 0; i < rows * cols; i++)
            {
                trasposeElements[i].Should().Be(matrix[i]);
            }
        }

        [Theory]
        [InlineData(new double[] { 1, 1, 1, 0, 0, 1, 0, 1, 0 }, 3, 3, new double[] { 1, -1, -1, 0, 0, 1, 0, 1, 0 } )]
        public void InvertMatrixReturnsInvertedMatrix(double[] elements, int rows, int cols, double[] inverseElements)
        {
            Matrix matrix = new(elements, rows, cols);

            Matrix invert = matrix.Invert();
            for (int i = 0; i < rows * cols; i++)
            {
                invert[i].Should().Be(inverseElements[i]);
            }
        }
    }
}
