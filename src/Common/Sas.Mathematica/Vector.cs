namespace Sas.Mathematica
{
    /// <summary>
    /// Class <c>Vector</c> models a vector in a three-dimensional space
    /// </summary>
    public class Vector
    {

        /// <summary>
        /// Component x of the vector
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Component y of the vector
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Component z of the vector
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Constructor of the vector
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector(double x = 0, double y = 0, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Magnitude of the vector
        /// </summary>
        /// <returns>Magnitude</returns>
        public double Magnitude() => Math.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>
        /// Vector zero 
        /// </summary>
        public static Vector Zero => new(0, 0, 0);

        /// <summary>
        /// Overloaded addition operator 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Sum of two vectors</returns>
        public static Vector operator +(Vector v1, Vector v2) => new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

        /// <summary>
        /// Overloaded subtraction operator 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Difference of two vectora</returns>
        public static Vector operator -(Vector v1, Vector v2) => new(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

        /// <summary>
        /// Overloaded multiplication operator (dot product)
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Dot product of two vectors</returns>
        public static double operator *(Vector v1, Vector v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;

        /// <summary>
        /// Overloaded multiplication operator by scalar
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="s">scalar</param>
        /// <returns></returns>
        public static Vector operator *(Vector v, double s) => new(s * v.X, s * v.Y, s * v.Z);

        /// <summary>
        /// Overloaded multiplication operator by scalar
        /// </summary>
        /// <param name="s">scalar</param>
        /// <param name="v">vector</param>
        /// <returns></returns>
        public static Vector operator *(double s, Vector v) => new(s * v.X, s * v.Y, s * v.Z);

        /// <summary>
        /// Overloaded operator for matrix operation
        /// </summary>
        /// <param name="m"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector operator *(Matrix m, Vector v)
        {
            if (m.GetDimension() != 3) throw new ArgumentException();
            double x = m[0] * v.X + m[1] * v.Y + m[2] * v.Z;
            double y = m[3] * v.X + m[4] * v.Y + m[5] * v.Z;
            double z = m[6] * v.X + m[7] * v.Y + m[8] * v.Z;
            return new Vector(x, y, z);
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
        public Vector CrossProduct(Vector v) => new(v.Y * Z - v.Z * Y, v.Z * X - v.X * Z, v.X * Y - v.Y * X);

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
    }
}
