namespace Sas.Mathematica.Service.Vectors
{
    /// <summary>
    /// Class <c>Vector</c> models a vector in a three-dimensional space
    /// </summary>
    public class Vector
    {
        private double[] _elements;
        
        /// <summary>
        /// Constructor of the vector
        /// </summary>
        /// <param name="elements"></param>
        public Vector(params double[] elements)
        {
            _elements = elements;
        }

        /// <summary>
        /// Constructor of the vector
        /// </summary>
        /// <param name="x">First component</param>
        /// <param name="y">Secound component</param>
        /// <param name="z">Third component</param>
        public Vector(double x = 0, double y = 0, double z = 0)
        {
            _elements = new double[3] {x, y, z};
        }

        /// <summary>
        /// Vector zero 
        /// </summary>
        public static Vector Zero => new(0, 0, 0);

        /// <summary>
        /// Component x of the vector
        /// </summary>
        public double X
        {
            get
            {
                return _elements.Length > 0 ? _elements[0] : 0;
            }
        }

        /// <summary>
        /// Component y of the vector
        /// </summary>
        public double Y 
        {
            get
            {
                return _elements.Length > 1 ? _elements[1] : 0;
            }
        }

        /// <summary>
        /// Component z of the vector
        /// </summary>
        public double Z
        {
            get
            {
                return _elements.Length > 2 ? _elements[2] : 0;
            }
        }

        /// <summary>
        /// Magnitude of the vector
        /// </summary>
        /// <returns>Magnitude</returns>
        public double Magnitude
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < _elements.Length; i++)
                {
                    sum += Math.Pow(_elements[i], 2);
                }
                return Math.Sqrt(sum);
            }
        }

        /// <summary>
        /// Returns length
        /// </summary>
        public int Length => _elements.Length;

        /// <summary>
        /// 
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
        /// Overloaded addition operator for two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>

        public static Vector operator +(Vector v1, Vector v2)
        {
            int longerSize = v1.Length > v2.Length ? v1.Length : v2.Length;
            int shorterSize = v1.Length < v2.Length ? v1.Length : v2.Length;
            double[] finalVector = new double[longerSize];
            Vector longerVector = v1.Length > v2.Length ? v1 : v2;
            for (int i = 0; i < longerSize; i++)
            {
                if (i < shorterSize)
                {
                    finalVector[i] = v1[i] + v2[i];
                }
                else
                {
                    finalVector[i] = longerVector[i];
                }
            }
            return new Vector(finalVector);
        }

        /// <summary>
        /// Overloaded subtraction operator for two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector operator -(Vector v1, Vector v2)
        {
            int longerSize = v1.Length > v2.Length ? v1.Length : v2.Length;
            int shorterSize = v1.Length < v2.Length ? v1.Length : v2.Length;
            double[] finalVector = new double[longerSize];
            Vector longerVector = v1.Length > v2.Length ? v1 : v2;
            for (int i = 0; i < longerSize; i++)
            {
                if (i < shorterSize)
                {
                    finalVector[i] = v1[i] - v2[i];
                }
                else
                {
                    finalVector[i] = longerVector[i];
                }
            }
            return new Vector(finalVector);
        }

        /// <summary>
        /// Overloaded multiplication operator for two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector operator *(Vector v1, Vector v2)
        {
            int longerSize = v1.Length > v2.Length ? v1.Length : v2.Length;
            int shorterSize = v1.Length < v2.Length ? v1.Length : v2.Length;
            double[] finalVector = new double[longerSize];
            Vector longerVector = v1.Length > v2.Length ? v1 : v2;
            for (int i = 0; i < longerSize; i++)
            {
                if (i < shorterSize)
                {
                    finalVector[i] = v1[i] * v2[i];
                }
                else
                {
                    finalVector[i] = longerVector[i];
                }
            }
            return new Vector(finalVector);
        }

        /// <summary>
        /// Overloaded multiplication operator for two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector operator *(Vector v1, double s)
        {
            for (int i = 0; i < v1.Length; i++)
            {
                v1[i] *= s;
            }
            return v1;
        }

        /// <summary>
        /// Overloaded multiplication operator for two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector operator *(double s, Vector v1)
        {
            return v1 * s;
        }

        /// <summary>
        /// Cross product of two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector CrossProduct(Vector v1, Vector v2) => new(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);

        /// <summary>
        /// Cross product of current vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector CrossProduct(Vector v) => new(Y * v.Z - Z * v.Y, Z * v.X - X * v.Z, X * v.Y - Y * v.X);

        /// <summary>
        /// Dot product of two vector
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Dot Product</returns>
        public static double DotProduct(Vector v1, Vector v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;

        public override string ToString()
        {
            return $"<{X}, {Y}, {Z}>";
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector vector &&
                   X == vector.X &&
                   Y == vector.Y &&
                   Z == vector.Z &&
                   Magnitude == vector.Magnitude &&
                   Length == vector.Length;
        }
    }
}
