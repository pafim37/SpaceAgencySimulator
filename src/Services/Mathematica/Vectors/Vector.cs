namespace Sas.Mathematica.Service.Vectors
{
    /// <summary>
    /// Class <c>Vector</c> models a vector in a three-dimensional space
    /// </summary>
    public class Vector
    {
        private double[] _elements;
        private bool _isNormalize;
        private double _magnitude;
        private readonly int _dim;

        #region constructors
        /// <summary>
        /// Constructor of the vector
        /// </summary>
        /// <param name="elements"></param>
        public Vector(params double[] elements)
        {
            _elements = elements;
            _dim = elements.Length;
            AssignFields();
        }

        /// <summary>
        /// Constructor of the vector
        /// </summary>
        /// <param name="x">First component</param>
        /// <param name="y">Secound component</param>
        /// <param name="z">Third component</param>
        public Vector(double x = 0, double y = 0, double z = 0)
        {
            _elements = [x, y, z];
            _dim = 3;
            AssignFields();
        }

        /// <summary>
        /// Vector zero 
        /// </summary>
        public static Vector Zero => new(0, 0, 0);

        /// <summary>
        /// Vector unit 
        /// </summary>
        public static Vector Ones => new(1, 1, 1);

        /// <summary>
        /// Vector X = <1, 0, 0>
        /// </summary>
        public static Vector Ox => new(1, 0, 0);

        /// <summary>
        /// Vector Y = <0, 1, 0>
        /// </summary>
        public static Vector Oy => new(0, 1, 0);

        /// <summary>
        /// Vector Z = <0, 0, 1>
        /// </summary>
        public static Vector Oz => new(0, 0, 1);
        

        #endregion

        #region properties
        /// <summary>
        /// Component x of the vector
        /// </summary>
        public double X
        {
            get
            {
                return _dim > 0 ? _elements[0] : 0;
            }
        }

        /// <summary>
        /// Component y of the vector
        /// </summary>
        public double Y
        {
            get
            {
                return _dim > 1 ? _elements[1] : 0;
            }
        }

        /// <summary>
        /// Component z of the vector
        /// </summary>
        public double Z
        {
            get
            {
                return _dim > 2 ? _elements[2] : 0;
            }
        }

        /// <summary>
        /// Returns length of the Vector
        /// </summary>
        public int Length => _dim;

        /// <summary>
        /// Magnitude of the vector
        /// </summary>
        /// <returns>Magnitude</returns>
        public double Magnitude => _magnitude;

        /// <summary>
        /// Returns whether the vector is normalized
        /// </summary>
        public bool IsNormalize => _isNormalize;
        #endregion

        #region operators
        /// <summary>
        /// Gets element of given index
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
            double[] newElements = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                newElements[i] = v1[i] * s;
            }
            return new(newElements);
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
        #endregion

        #region static
        public static Vector CrossProduct(Vector v1, Vector v2) => new(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        #endregion

        #region public functions
        /// <summary>
        /// Returns vector elements
        /// </summary>
        /// <returns></returns>
        public double[] GetElements()
        {
            return _elements;
        }


        /// <summary>
        /// Normalize the vector. Has no effect if vector is currently normalized
        /// </summary>
        public void Normalize()
        {
            if (!_isNormalize)
            {
                _elements = _elements.Select(element => element / _magnitude).ToArray();
                _isNormalize = true;
            }
        }

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
        #endregion

        #region overrides
        public override string ToString()
        {
            return $"<{X}, {Y}, {Z}>";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
        #endregion

        #region privates
        private double CalculateMagnitude()
        {
            return Math.Sqrt(_elements.Sum(element => element * element));
        }

        private bool CheckIfNormalize()
        {
            bool isNormalize = true;
            foreach (var element in _elements)
            {
                isNormalize &= element == element / _magnitude;
            }
            return isNormalize;
        }

        private void AssignFields()
        {
            _magnitude = CalculateMagnitude();
            _isNormalize = CheckIfNormalize();
        }
        #endregion
    }
}
