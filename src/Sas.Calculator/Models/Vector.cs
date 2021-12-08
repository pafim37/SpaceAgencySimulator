using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Calculator.Models
{
    /// <summary>
    /// Class <c>Vector</c> models a vector in a three-dimensional space
    /// </summary>
    public class Vector
    {

        /// <summary>
        /// Component x of the vector
        /// </summary>
        public double X { get ; set; } 

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
        /// Overloaded addition operator 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Sum of two vectors</returns>
        public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

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
        /// <param name="a">scalar</param>
        /// <returns></returns>
        public static Vector operator *(Vector v, double s) => new Vector(s * v.X, s * v.Y, s * v.Z);

        /// <summary>
        /// Overloaded multiplication operator by scalar
        /// </summary>
        /// <param name="s">scalar</param>
        /// <param name="v">vector</param>
        /// <returns></returns>
        public static Vector operator *(double s, Vector v) => new Vector(s * v.X, s * v.Y, s * v.Z);

        /// <summary>
        /// Cross product of two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector CrossProduct(Vector v1, Vector v2) => new Vector(v1.Y* v2.Z - v1.Z* v2.Y, v1.Z* v2.X - v1.X* v2.Z, v1.X* v2.Y - v1.Y* v2.X);

        /// <summary>
        /// Cross product of current vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector CrossProduct(Vector v) => new Vector(v.Y * this.Z - v.Z * this.Y, v.Z * this.X - v.X * this.Z, v.X * this.Y - v.Y * this.X);

        /// <summary>
        /// Dot product of two vector
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>Dot Product</returns>
        public static double DotProduct(Vector v1, Vector v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;

        /// <summary>
        /// Magnitude of the vector
        /// </summary>
        /// <returns>Magnitude</returns>
        public double Magnitude() => Math.Sqrt( X*X + Y*Y + Z*Z );

        public override string ToString()
        {
            return $"<{X}, {Y}, {Z}>";
        }
    }
}
