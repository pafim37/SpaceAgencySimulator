using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Calculator.Models
{
    public class Matrix
    {
        /// <summary>
        /// Elements of the matrix
        /// </summary>
        private double[] _elements;


        /// <summary>
        /// Dimension of the matrix
        /// </summary>
        private int _dimension;
        public int Dimension
        {
            get => _dimension;
        }

        /// <summary>
        ///  Determinant of the matrix
        /// </summary>
        public double Determinant
        {
            get => CalculateDeterminant(this);
        }

        /// <summary>
        /// Constructor of the matrix
        /// </summary>
        /// <param name="elements">elements of the matrix</param>
        /// <exception cref="ArgumentException"></exception>
        public Matrix(params double[] elements)
        {
            if(IselementsValid(elements))
            {
                _dimension = (int)Math.Sqrt(elements.Length);
                _elements = elements;
            }
            else
            {
                throw new ArgumentException("Matrix is not square");
            }
        }

        /// <summary>
        /// Get the mathematical element at position row,col where 1<=row<=dim 1<=col<=dim
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>Element of the matrix</returns>
        public double GetElementAtPosition(int row, int col) => _elements[--row * Dimension + --col];
        
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
        /// Overloaded multiplication operator scalar and matrix
        /// </summary>
        /// <param name="s"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix operator *(double s, Matrix matrix)
        {
            double[] tmpMatrixelements = new double[matrix.Dimension*matrix.Dimension];
            for (int i = 0; i < matrix.Dimension*matrix.Dimension; i++)
            {
                tmpMatrixelements[i] = s * matrix[i];
            }
            return new Matrix(tmpMatrixelements);
        }

        /// <summary>
        /// Transpose of the matrix 
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix Transpose(Matrix matrix)
        {
            for (int row = 0; row < matrix.Dimension; row++)
            {
                for (int col = 0; col < row; col++)
                {
                    double tmp = matrix[row * matrix.Dimension + col];
                    matrix[row * matrix.Dimension + col] = matrix[col * matrix.Dimension + row];
                    matrix[col * matrix.Dimension + row] = tmp;
                }
            }
            return matrix;
        }

        /// <summary>
        /// Invert matrix
        /// </summary>
        public void Invert()
        {
            double[] tmpElements = new double[_dimension * _dimension];
            for (int row = 0; row < _dimension; row++)
            {
                for (int col = 0; col < _dimension; col++)
                {
                    var minor = CreateMinor(this, row, col);
                    tmpElements[row * _dimension + col] = Math.Pow(-1, row + col) * minor.Determinant;
                }
            }
            Matrix cofactor = new Matrix(tmpElements);
            Matrix adjugate = Transpose(cofactor);
            Console.WriteLine(adjugate);
            Matrix invertedMatrix =  1 / Determinant * adjugate;
            _elements = GetElements(invertedMatrix).ToArray();
        }

        public override string? ToString()
        {
            string result = string.Empty;
            for (int row = 0; row < Dimension; row++)
            {
                for (int col = 0; col < Dimension; col++)
                {
                    result += _elements[row * Dimension + col] + ", ";
                }
                result += "\n";
            }
            return result;
        }
        private double CalculateDeterminant(Matrix matrix)
        {
            if (matrix.Dimension == 1) return matrix[0];
            else if (matrix.Dimension == 2) return matrix[0] * matrix[3] - matrix[1] * matrix[2];
            else
            {
                double det = 0.0;
                for (int i = 0; i < matrix.Dimension; i++)
                {
                    Matrix minor = CreateMinor(matrix, matrix.Dimension-1, i);
                    det += Math.Pow(-1, matrix.Dimension + i + 1) * matrix[ (int)((matrix.Dimension - 1) * matrix.Dimension + i)] * CalculateDeterminant(minor);
                }
                return det;
            }
        }

        private Matrix CreateMinor(Matrix matrix, int i, int j)
        {
            double[] minorelements = CreateMinorelements(matrix, i, j).ToArray();
            return new Matrix(minorelements);
        }

        private IEnumerable<double> CreateMinorelements(Matrix matrix, int i, int j)
        {
            for (int row = 0; row < matrix.Dimension; row++) 
            {
                if (row == i) continue;
                for (int col = 0; col < matrix.Dimension; col++)
                {
                    if (col == j) continue;
                    yield return matrix[row * matrix.Dimension + col];
                }
            }
        }

        /// <summary>
        /// Get all elements of the matrix 
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>elements as enumerable collection</returns>
        private IEnumerable<double> GetElements(Matrix matrix)
        {
            for (int row = 0; row < matrix.Dimension; row++)
            {
                for (int col = 0; col < matrix.Dimension; col++)
                {
                    yield return matrix[row * matrix.Dimension + col];
                }
            }
        }

        /// <summary>
        /// Check if matrix has at least one element and is square matrix
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        private bool IselementsValid(double[] elements)
        {
            double dim = Math.Sqrt(elements.Length);
            return elements != null
                && dim * dim == (int)(elements.Length)
                && dim > 1;
        }
    }
}
