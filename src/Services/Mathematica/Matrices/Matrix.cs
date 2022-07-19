namespace Sas.Mathematica
{
    public class Matrix
    {
        #region Privates Fields

        private double[] _elements;
        private int _dim;
        private int _numberOfRows;
        private int _numberOfColumns;
        private bool _squareMatrix;

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
                _dim = (int)Math.Sqrt(elements.Length);
            }
            else
            {
                _squareMatrix = false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all matrix element
        /// </summary>
        /// <returns></returns>
        public double[] GetAllElements() => _elements!;

        /// <summary>
        /// Gets number of rows
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfRows() => _numberOfRows;

        /// <summary>
        /// Gets number of columns
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfColumns() => _numberOfColumns;

        /// <summary>
        /// Gets dimension of the matrix if matrix is square
        /// </summary>
        /// <returns></returns>
        public int? GetDimension()
        {
            return _squareMatrix ? _dim : null;
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
                if ( (row >= 1 && row <= _numberOfRows) && (col >= 1 && col <= _numberOfColumns)) return _elements[--row * _numberOfColumns + --col];
                else throw new IndexOutOfRangeException();
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
            set => _elements[i] = value;
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
                    var minor = CreateMinor(this, row, col);
                    tmpElements[row * dim + col] = Math.Pow(-1, row + col) * minor.GetDeterminant();
                }
            }
            Matrix cofactor = new Matrix(tmpElements, dim, dim);
            Matrix adjugate = cofactor.Transpose();
            Matrix invertedMatrix = (1 / det) * adjugate;
            _elements = invertedMatrix.GetAllElements();
            return this;
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

            if (dim == 1) return _elements![0];
            else if (dim == 2) return _elements![0] * _elements[3] - _elements[1] * _elements[2];
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
        /// Create a minor of matrix by remove row and column
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="i">index of row to remove</param>
        /// <param name="j">index of col to remove</param>
        /// <returns></returns>
        private Matrix CreateMinor(Matrix matrix, int i, int j)
        {
            double[] minorelements = CreateMinorElements(matrix, i, j).ToArray();
            return new Matrix(minorelements, matrix.GetDimension()!.Value-1, matrix.GetDimension()!.Value-1);
        }

        private IEnumerable<double> CreateMinorElements(Matrix matrix, int i, int j)
        {
            int dim = matrix.GetDimension().Value;
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
        private void AreArgumentsValid(double[] elements, int numberOfRows, int numberOfColumns)
        {
            if (elements is null) throw new ArgumentNullException(nameof(elements));

            if (numberOfRows <= 0) throw new ArgumentOutOfRangeException(nameof(numberOfRows));
            if (numberOfColumns <= 0) throw new ArgumentOutOfRangeException(nameof(numberOfColumns));

            if (numberOfRows * numberOfColumns != elements.Length) throw new ArgumentOutOfRangeException("Bad number of elements for provided dimensions of matrix");
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
            int dim = matrix.GetDimension().Value;
            double[] tmpMatrixElements = new double[dim*dim];
            for (int i = 0; i < dim*dim; i++)
            {
                tmpMatrixElements[i] = s * matrix[i];
            }

            return new Matrix(tmpMatrixElements, dim, dim);
        }

        public static Matrix operator *(Matrix matrix, double s)
        {
            return s * matrix;
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
                    result += _elements![row * (_numberOfRows-1) + col] + ", ";
                }
                result += "\n";
            }
            return result;
        }

        public override bool Equals(object? obj)
        {
            return obj is Matrix matrix &&
                   _elements.SequenceEqual(matrix.GetAllElements()) &&
                   _numberOfRows == matrix.GetNumberOfRows() &&
                   _numberOfColumns == matrix.GetNumberOfColumns();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_numberOfRows, _numberOfColumns, _elements);
        }

        #endregion
    }
}
