using System.Numerics;

namespace Sas.Mathematica.Service.Vectors
{
    public class Vector(double x = 0, double y = 0, double z = 0) : IEquatable<Vector>
    {

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

        public double X { get; } = x;
        public double Y { get; } = y;
        public double Z { get; } = z;
        public double Magnitude => Math.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>
        /// Converts to single precision for interoperability. This conversion can lose precision.
        /// </summary>
        public Vector3 GetVector3() => new((float)X, (float)Y, (float)Z);
        public static Vector AsVector(Vector3 vector) => new(vector.X, vector.Y, vector.Z);
        public static Vector operator +(Vector v1, Vector v2) => new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector operator -(Vector v1, Vector v2) => new(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static Vector operator *(Vector v1, double s) => new(v1.X * s, v1.Y * s, v1.Z * s);
        public static Vector operator *(double s, Vector v1) => v1 * s;
        public static Vector operator -(Vector v) => new(-v.X, -v.Y, -v.Z);
        public static bool operator ==(Vector? v1, Vector? v2) => ReferenceEquals(v1, v2) || (v1?.Equals(v2) ?? false);
        public static bool operator !=(Vector? v1, Vector? v2) => !(v1 == v2);
        public static Vector CrossProduct(Vector v1, Vector v2) => new(
            v1.Y * v2.Z - v1.Z * v2.Y,
            v1.Z * v2.X - v1.X * v2.Z,
            v1.X * v2.Y - v1.Y * v2.X);
        public static double DotProduct(Vector v1, Vector v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;

        public bool Equals(Vector? other) => other is not null
            && X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        public override bool Equals(object? obj) => obj is Vector other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
    }
}
