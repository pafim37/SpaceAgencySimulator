using Sas.Common;
using Sas.Mathematica;
using Sas.Mathematica.Service.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Domain
{
    public class LeastSquaresEstimation
    {
        private Matrix _matrix;
        public LeastSquaresEstimation()
        {
            _matrix = new Matrix(new double[] { 12, -51, 4, 6, 167, -68, -4, 24, -41 }, 3, 3);
            int m = _matrix.GetNumberOfColumns();
            Matrix R = new Matrix(new double[m * m], m, m);
            for (int col = 1; col <= m; col++)
            {
                R[col, col] = Math.Round(_matrix.GetColumn(col).ToVector().Magnitude);
                for (int row = 1; row <= m; row++)
                {
                    _matrix[row, col] = _matrix[row, col] / R[col, col];
                }
                for (int k = col + 1; k < m; k++)
                {
                    R[col, k] = Math.Round(Vector.DotProduct(_matrix.GetColumn(col).ToVector(), _matrix.GetColumn(k).ToVector()));
                    for (int i = 1; i < m; i++)
                    {
                        _matrix[i, k] = _matrix[i,k] - _matrix[i,col] * R[col, k];
                    }
                }
            }

            Console.WriteLine(R.ToString());
        }
    }
}
