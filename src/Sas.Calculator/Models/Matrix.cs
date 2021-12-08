using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Calculator.Models
{
    public class Matrix
    {
        private double[] _elements;
        private int _dim;
        
        public double[] GetElements() => _elements;
        public int GetDimension() => _dim;

        /// <summary>
        /// Get element of matrix at position (row, col)
        /// </summary>
        /// <param name="row">number of row</param>
        /// <param name="col">number of column</param>
        /// <returns>value</returns>
        public double GetElementAtPosition(int row, int col) => _elements[--row * _dim + --col];

        /// <summary>
        ///  Determinant of the matrix
        /// </summary>
        public double Determinant => CalculateDeterminant();

        public Matrix(params double[] elements)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));
            if (!IsMatrixSquare(elements))
            {
                throw new Exception("Matrix is not square");
            }
            else
            {
                _elements = elements;
                _dim = (int)Math.Sqrt(elements.Length);
            }
        }

        /// <summary>
        /// Define the indexer to allow client to use [] notation
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
            for (int row = 0; row < _dim; row++)
            {
                for (int col = 0; col < _dim; col++)
                {
                    double tmp = this[row * _dim + col];
                    this[row * _dim + col] = this[col * _dim + row];
                    this[col * _dim + row] = tmp;
                }
            }
            return this;
        }

        /// <summary>
        /// Invert matrix
        /// </summary>
        public Matrix Invert()
        {
            double det = this.Determinant;
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
                    tmpElements[row * dim + col] = Math.Pow(-1, row + col) * minor.Determinant;
                }
            }
            Matrix cofactor = new Matrix(tmpElements);
            Matrix adjugate = cofactor.Transpose();
            Matrix invertedMatrix =  1 / det * adjugate;
            _elements = invertedMatrix.GetElements();
            return this;
        }

        /// <summary>
        /// Calculate a determinant of the matrix
        /// </summary>
        /// <returns>determinant as a double</returns>
        private double CalculateDeterminant()
        {
            int dim = this.GetDimension();

            if (dim == 1) return _elements[0];
            else if (dim == 2) return _elements[0] * _elements[3] - _elements[1] * _elements[2];
            else
            {
                double det = 0.0;
                for (int i = 0; i < dim; i++)
                {
                    Matrix minor = CreateMinor(this, dim-1, i);
                    det += Math.Pow(-1, dim + i + 1) * this[ (int)((dim - 1) * dim + i)] * minor.Determinant;
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
            double[] minorelements = CreateMinorelements(matrix, i, j).ToArray();
            return new Matrix(minorelements);
        }
        
        private IEnumerable<double> CreateMinorelements(Matrix matrix, int i, int j)
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
        /// Check if matrix is square
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        private static bool IsMatrixSquare(double[] elements)
        {
            int dim = (int)Math.Sqrt(elements.Length);
            return dim * dim == elements.Length;
        }

        public override string? ToString()
        {
            int dim = this.GetDimension();
            string result = string.Empty;
            for (int row = 0; row < dim; row++)
            {
                for (int col = 0; col < dim; col++)
                {
                    result += _elements[row * dim + col] + ", ";
                }
                result += "\n";
            }
            return result;
        }

        /// <summary>
        /// Overloaded multiplication operator scalar and matrix
        /// </summary>
        /// <param name="s"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix operator *(double s, Matrix matrix)
        {
            int dim = matrix.GetDimension();
            double[] tmpMatrixElements = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                tmpMatrixElements[i] = s * matrix[i];
            }

            return new Matrix(tmpMatrixElements);
        }

        public static bool operator ==(Matrix? left, Matrix? right)
        {
            if (left.GetDimension() != right.GetDimension())
            {
                return false;
            }

            bool result = true;
            for (int i = 0; i < left.GetDimension(); i++)
            {
                result &= left[i] == right[i];
            }
            return result;
        }

        public static bool operator !=(Matrix? left, Matrix? right)
        {
            return !(left == right);
        }
    }
}
