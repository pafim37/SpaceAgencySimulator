using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Service.Matrices
{
    public class Matrix
    {
        #region Privates Fields
        private double[] _elements;
        private readonly int _dim;
        private int _numberOfRows;
        private int _numberOfColumns;
        private readonly bool _squareMatrix;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor of the matrix
        /// </summary>
        /// <param name="numberOfRows"></param>
        /// <param name="numberOfColumns"></param>
        /// <param name="elements"></param>
        public Matrix(double[] elements, int numberOfRows, int numberOfColumns)
        {
            AreArgumentsValid(elements, numberOfRows, numberOfColumns);
            _numberOfRows = numberOfRows;
            _numberOfColumns = numberOfColumns;
            _elements = elements;
            if (numberOfRows == _numberOfColumns)
            {
                _squareMatrix = true;
                _dim = numberOfRows;
            }
            else
            {
                _squareMatrix = false;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets number of rows
        /// </summary>
        /// <returns></returns>
        public int RowsNumber { get { return _numberOfRows; } }

        /// <summary>
        /// Gets number of columns
        /// </summary>
        /// <returns></returns>
        public int ColumnsNumber { get { return _numberOfColumns; } }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all matrix element
        /// </summary>
        /// <returns></returns>
        public double[] GetAllElements() => _elements;

        /// <summary>
        /// Returns dimension of the matrix if matrix is square
        /// </summary>
        /// <returns></returns>
        public int GetDimension()
        {
            return _squareMatrix ? _dim : throw new Exception("Matrix is irregular");
        }

        /// <summary>
        /// Returns single (col-th) column 
        /// </summary>
        /// <param name="col">number of column</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public double[] GetColumn(int col)
        {
            if (col < 0 || col > _numberOfColumns) throw new IndexOutOfRangeException(nameof(col));
            double[] column = new double[_numberOfRows];
            for (int c = 0; c < _numberOfRows; c++)
            {
                int index = c * _numberOfRows + col;
                column[c] = _elements[--index];
            }
            return column;
        }

        /// <summary>
        /// Gets Determinant of the matrix
        /// </summary>
        public double GetDeterminant() => CalculateDeterminant();

        /// <summary>
        /// Define the indexer to allow use [] notation
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public double this[int row, int col]
        {
            get
            {
                if (row >= 1 && row <= _numberOfRows && col >= 1 && col <= _numberOfColumns)
                {
                    return _elements[--row * _numberOfColumns + --col];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (row >= 1 && row <= _numberOfRows && col >= 1 && col <= _numberOfColumns)
                {
                    _elements[--row * _numberOfColumns + --col] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }

        }

        /// <summary>
        /// Define the indexer to allow use [] notation
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public double this[int i]
        {
            get
            {
                if (i >= 0 && i < _elements.Length) return _elements[i];
                else throw new IndexOutOfRangeException();
            }
            set
            {
                if (i >= 0 && i < _elements.Length) _elements[i] = value;
                else throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Transpose of the matrix 
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Matrix Transpose()
        {
            double[] transposeElements = new double[_elements.Length];
            for (int row = 0; row < _numberOfRows; row++)
            {
                for (int col = 0; col < _numberOfColumns; col++)
                {
                    transposeElements[col * _numberOfRows + row] = _elements[row * _numberOfColumns + col];
                }
            }

            for (int i = 0; i < _elements.Length; i++)
            {
                _elements[i] = transposeElements[i];
            }

            (_numberOfRows, _numberOfColumns) = (_numberOfColumns, _numberOfRows);
            return this;
        }

        /// <summary>
        /// Invert matrix
        /// </summary>
        public Matrix Invert()
        {
            if (!_squareMatrix) throw new InvalidOperationException("Matrix is no square");
            double det = CalculateDeterminant();
            if (det == 0)
            {
                throw new Exception("Irreversible matrix");
            }

            int dim = _dim;
            double[] tmpElements = new double[dim * dim];
            for (int row = 0; row < dim; row++)
            {
                for (int col = 0; col < dim; col++)
                {
                    Matrix? minor = CreateMinor(this, row, col);
                    tmpElements[row * dim + col] = Math.Pow(-1, row + col) * minor.GetDeterminant();
                }
            }
            Matrix cofactor = new(tmpElements, dim, dim);
            Matrix adjugate = cofactor.Transpose();
            Matrix invertedMatrix = 1 / det * adjugate;
            _elements = invertedMatrix.GetAllElements();
            return this;
        }
        public Vector ToVector()
        {
            return new Vector(_elements);
        }
        #endregion

        #region Privates Methods
        /// <summary>
        /// Calculate a determinant of the matrix
        /// </summary>
        /// <returns>determinant as a double</returns>
        private double CalculateDeterminant()
        {
            if (!_squareMatrix)
            {
                throw new Exception("Matrix is no square");
            }

            int dim = _dim;

            if (dim == 1) return _elements[0];
            else if (dim == 2) return _elements[0] * _elements[3] - _elements[1] * _elements[2];
            else
            {
                double det = 0.0;
                for (int i = 0; i < dim; i++)
                {
                    Matrix minor = CreateMinor(this, dim - 1, i);
                    det += Math.Pow(-1, dim + i + 1) * this[(dim - 1) * dim + i] * minor.GetDeterminant();
                }
                return det;
            }
        }

        /// <summary>
        /// Creates a minor of matrix by remove row and column
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="i">index of row to remove</param>
        /// <param name="j">index of col to remove</param>
        /// <returns></returns>
        private static Matrix CreateMinor(Matrix matrix, int i, int j)
        {
            double[] minorelements = CreateMinorElements(matrix, i, j).ToArray();
            return new Matrix(minorelements, matrix.GetDimension() - 1, matrix.GetDimension() - 1);
        }

        private static IEnumerable<double> CreateMinorElements(Matrix matrix, int i, int j)
        {
            int dim = matrix.GetDimension();
            for (int row = 0; row < dim; row++)
            {
                if (row == i) continue;
                for (int col = 0; col < dim; col++)
                {
                    if (col == j) continue;
                    yield return matrix[row * dim + col];
                }
            }
        }

        /// <summary>
        /// Check if arguments are valid
        /// </summary>
        /// <returns></returns>
        private static void AreArgumentsValid(double[] elements, int numberOfRows, int numberOfColumns)
        {
            ArgumentNullException.ThrowIfNull(elements);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfRows);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfColumns);
            if (numberOfRows * numberOfColumns != elements.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(elements), "Bad number of elements for provided dimensions of matrix");
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Overloaded multiplication operator scalar and matrix
        /// </summary>
        /// <param name="s"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix operator *(double s, Matrix matrix)
        {
            int rows = matrix.RowsNumber;
            int cols = matrix.ColumnsNumber;
            double[] tmpMatrixElements = new double[rows * cols];
            for (int i = 0; i < rows * cols; i++)
            {
                tmpMatrixElements[i] = s * matrix[i];
            }

            return new Matrix(tmpMatrixElements, rows, cols);
        }

        /// <summary>
        /// Overloaded multiplication operator matrix and scalar
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix matrix, double s)
        {
            return s * matrix;
        }

        /// <summary>
        /// Overloaded multiplication of matrix and matrix 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Matrix operator *(Matrix A, Matrix B)
        {
            int rowsA = A.RowsNumber;
            int colsA = A.ColumnsNumber;
            int rowsB = B.RowsNumber;
            int colsB = B.ColumnsNumber;

            if (colsA != rowsB)
                throw new InvalidOperationException("Number of columns of the first matrix is not equal to the numbers of the rows of the secound matrix");

            Matrix result = new(new double[rowsA * colsB], rowsA, colsB);

            for (int i = 1; i <= rowsA; i++)
            {
                for (int j = 1; j <= colsB; j++)
                {
                    for (int k = 1; k <= colsA; k++)
                    {
                        result[i, j] += A[i, k] * B[k, j];
                    }
                }
            }

            return result;
        }
        #endregion

        #region Overrides
        public override string? ToString()
        {
            string result = string.Empty;
            for (int row = 0; row < _numberOfRows; row++)
            {
                for (int col = 0; col < _numberOfColumns; col++)
                {
                    result += _elements[row * (_numberOfRows - 1) + col] + ", ";
                }
                result += "\n";
            }
            return result;
        }

        public override bool Equals(object? obj)
        {
            return obj is Matrix matrix
                && _elements.SequenceEqual(matrix.GetAllElements())
                && _numberOfRows == matrix.RowsNumber
                && _numberOfColumns == matrix.ColumnsNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_elements, _numberOfRows, _numberOfColumns);
        }

        #endregion
    }
}
