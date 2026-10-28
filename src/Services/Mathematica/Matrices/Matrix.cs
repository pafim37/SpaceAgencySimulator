using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using SasVector = Sas.Mathematica.Service.Vectors.Vector;

namespace Sas.Mathematica.Service.Matrices
{
    /// <summary>A mutable matrix backed by Math.NET Numerics, exposing row-major arrays.</summary>
    /// <remarks>Transpose and Invert mutate this instance. Do not mutate a matrix used as a hash key.</remarks>
    public class Matrix
    {
        private Matrix<double> _values;

        /// <summary>Copies row-major elements into a matrix with positive dimensions.</summary>
        /// <exception cref="ArgumentNullException">Elements are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Dimensions or element count are invalid.</exception>
        public Matrix(double[] elements, int numberOfRows, int numberOfColumns)
        {
            ArgumentNullException.ThrowIfNull(elements);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfRows);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfColumns);
            if ((long)numberOfRows * numberOfColumns != elements.Length)
                throw new ArgumentOutOfRangeException(nameof(elements), "Element count must equal rows times columns.");
            _values = DenseMatrix.Create(numberOfRows, numberOfColumns,
                (row, col) => elements[row * numberOfColumns + col]);
        }

        // Only newly allocated Math.NET operation results are passed here, so no extra clone is needed.
        private Matrix(Matrix<double> values) => _values = values;

        /// <summary>The number of rows.</summary>
        public int RowsNumber => _values.RowCount;
        /// <summary>The number of columns.</summary>
        public int ColumnsNumber => _values.ColumnCount;
        /// <summary>Returns an independent copy of the elements in row-major order.</summary>
        public double[] GetAllElements() => _values.ToRowMajorArray();

        /// <summary>Returns the size of a square matrix.</summary>
        /// <exception cref="InvalidOperationException">The matrix is not square.</exception>
        public int GetDimension()
        {
            if (RowsNumber != ColumnsNumber) throw new InvalidOperationException("The matrix must be square.");
            return RowsNumber;
        }

        /// <summary>Copies a column selected by a one-based index.</summary>
        /// <exception cref="IndexOutOfRangeException">The column is outside 1..ColumnsNumber.</exception>
        public double[] GetColumn(int col)
        {
            if (col < 1 || col > ColumnsNumber) throw new IndexOutOfRangeException(nameof(col));
            return _values.Column(col - 1).ToArray();
        }

        /// <summary>Gets or sets an element using one-based row and column indices.</summary>
        /// <exception cref="IndexOutOfRangeException">Either index is outside the matrix.</exception>
        public double this[int row, int col]
        {
            get { ValidateIndices(row, col); return _values[row - 1, col - 1]; }
            set { ValidateIndices(row, col); _values[row - 1, col - 1] = value; }
        }

        /// <summary>Gets or sets a row-major element using a zero-based linear index.</summary>
        /// <exception cref="IndexOutOfRangeException">The index is outside the matrix.</exception>
        public double this[int i]
        {
            get { ValidateLinearIndex(i); return _values[i / ColumnsNumber, i % ColumnsNumber]; }
            set { ValidateLinearIndex(i); _values[i / ColumnsNumber, i % ColumnsNumber] = value; }
        }

        /// <summary>Transposes this matrix in place using Math.NET and returns this instance.</summary>
        public Matrix Transpose()
        {
            _values = _values.Transpose();
            return this;
        }

        /// <summary>Calculates a square matrix's determinant with Math.NET's pivoted LU decomposition.</summary>
        /// <remarks>No absolute epsilon is applied. A determinant can underflow or overflow even for an invertible matrix.</remarks>
        /// <exception cref="InvalidOperationException">The matrix is not square or contains nonfinite elements.</exception>
        public double GetDeterminant()
        {
            ValidateFactorizationInput();
            return _values.Determinant();
        }

        /// <summary>Replaces this matrix with its inverse using Math.NET's pivoted LU, returning this instance.</summary>
        /// <remarks>
        /// A zero LU pivot is rejected without using a determinant threshold. No condition number is estimated;
        /// callers handling ill-conditioned systems should verify the residual. Failure leaves this instance unchanged.
        /// </remarks>
        /// <exception cref="InvalidOperationException">The matrix is nonsquare, singular, or the result is nonfinite.</exception>
        public Matrix Invert()
        {
            ValidateFactorizationInput();
            var factor = _values.LU();
            Matrix<double> upper = factor.U;
            if (upper.Enumerate().Any(value => !double.IsFinite(value)))
                throw new InvalidOperationException("LU cannot be represented with finite components.");
            // Testing the determinant would incorrectly reject small invertible matrices when it underflows.
            for (int i = 0; i < RowsNumber; i++)
                if (upper[i, i] == 0) throw new InvalidOperationException("The matrix is singular.");
            Matrix<double> inverse = factor.Inverse();
            if (inverse.Enumerate().Any(value => !double.IsFinite(value)))
                throw new InvalidOperationException("The inverse cannot be represented with finite components.");
            _values = inverse;
            return this;
        }

        /// <summary>Copies the row-major elements to an independent vector, preserving their order.</summary>
        public SasVector ToVector() => new(GetAllElements());

        /// <summary>Multiplies a matrix by an N-dimensional column vector using Math.NET.</summary>
        /// <exception cref="ArgumentException">The vector length differs from the number of columns.</exception>
        public static SasVector operator *(Matrix matrix, SasVector vector)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            ArgumentNullException.ThrowIfNull(vector);
            if (matrix.ColumnsNumber != vector.Length)
                throw new ArgumentException("Vector length must match the matrix columns.", nameof(vector));
            Vector<double> input = new DenseVector(vector.GetElements());
            return new SasVector((matrix._values * input).ToArray());
        }

        /// <summary>Returns a new matrix with every component multiplied by the scalar.</summary>
        public static Matrix operator *(double s, Matrix matrix) => new(matrix._values * s);
        /// <summary>Returns a new matrix with every component multiplied by the scalar.</summary>
        public static Matrix operator *(Matrix matrix, double s) => s * matrix;

        /// <summary>Returns the Math.NET matrix product without modifying either operand.</summary>
        /// <exception cref="InvalidOperationException">The inner dimensions differ.</exception>
        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.ColumnsNumber != right.RowsNumber)
                throw new InvalidOperationException("The inner matrix dimensions must match.");
            return new(left._values * right._values);
        }

        /// <summary>Formats rows in storage order using the current culture and LF row separators.</summary>
        public override string ToString()
        {
            StringBuilder text = new();
            for (int row = 0; row < RowsNumber; row++)
            {
                for (int col = 0; col < ColumnsNumber; col++)
                    text.Append(_values[row, col]).Append(", ");
                text.Append('\n');
            }
            return text.ToString();
        }

        /// <summary>Compares dimensions and all components exactly.</summary>
        public override bool Equals(object? obj) => obj is Matrix matrix
            && RowsNumber == matrix.RowsNumber && ColumnsNumber == matrix.ColumnsNumber
            && _values.Enumerate().SequenceEqual(matrix._values.Enumerate());

        /// <summary>Hashes dimensions and all components; do not mutate a matrix used as a hash key.</summary>
        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(RowsNumber);
            hash.Add(ColumnsNumber);
            foreach (double value in _values.Enumerate()) hash.Add(value);
            return hash.ToHashCode();
        }

        private void ValidateIndices(int row, int col)
        {
            if (row < 1 || row > RowsNumber || col < 1 || col > ColumnsNumber) throw new IndexOutOfRangeException();
        }

        private void ValidateLinearIndex(int i)
        {
            if (i < 0 || i >= (long)RowsNumber * ColumnsNumber) throw new IndexOutOfRangeException();
        }

        private void ValidateFactorizationInput()
        {
            GetDimension();
            if (_values.Enumerate().Any(value => !double.IsFinite(value)))
                throw new InvalidOperationException("LU requires finite matrix components.");
        }
    }
}
