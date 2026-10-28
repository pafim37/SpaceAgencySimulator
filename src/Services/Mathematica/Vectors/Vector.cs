using Sas.Mathematica.Service.Matrices;

namespace Sas.Mathematica.Service.Vectors
{
    /// <summary>A mutable N-dimensional vector with full-precision components.</summary>
    /// <remarks>Arrays are copied. Do not mutate a vector while it is a hash key.</remarks>
    public class Vector
    {
        private readonly double[] _elements;
        private double? _magnitude;

        /// <summary>Copies all supplied components, including an empty vector.</summary>
        public Vector(params double[] elements) : this(elements, copy: true) { }

        private Vector(double[] elements, bool copy)
        {
            ArgumentNullException.ThrowIfNull(elements);
            _elements = copy ? (double[])elements.Clone() : elements;
        }

        /// <summary>Creates a 3D vector with omitted coordinates set to zero.</summary>
        public Vector(double x = 0, double y = 0, double z = 0) : this([x, y, z], copy: false) { }

        /// <summary>The 3D zero vector.</summary>
        public static Vector Zero => new(0, 0, 0);
        /// <summary>The vector (1, 1, 1), whose magnitude is sqrt(3).</summary>
        public static Vector Ones => new(1, 1, 1);
        /// <summary>The positive X unit vector.</summary>
        public static Vector Ox => new(1, 0, 0);
        /// <summary>The positive Y unit vector.</summary>
        public static Vector Oy => new(0, 1, 0);
        /// <summary>The positive Z unit vector.</summary>
        public static Vector Oz => new(0, 0, 1);

        /// <summary>The first component, or zero if absent.</summary>
        public double X => ComponentOrZero(0);
        /// <summary>The second component, or zero if absent.</summary>
        public double Y => ComponentOrZero(1);
        /// <summary>The third component, or zero if absent.</summary>
        public double Z => ComponentOrZero(2);
        /// <summary>The number of components.</summary>
        public int Length => _elements.Length;
        /// <summary>The Euclidean norm across all components, recalculated lazily after mutation.</summary>
        public double Magnitude => _magnitude ??= CalculateMagnitude();
        /// <summary>Whether the magnitude differs from one by at most 1e-12.</summary>
        public bool IsNormalize => Math.Abs(Magnitude - 1) <= 1e-12;

        /// <summary>Gets or sets a component using a zero-based index.</summary>
        /// <exception cref="IndexOutOfRangeException">The index is outside the vector.</exception>
        public double this[int i]
        {
            get => _elements[i];
            set { _elements[i] = value; _magnitude = null; }
        }

        /// <summary>Adds vectors, treating missing components as zero.</summary>
        public static Vector operator +(Vector v1, Vector v2)
        {
            double[] result = new double[Math.Max(v1.Length, v2.Length)];
            for (int i = 0; i < result.Length; i++)
                result[i] = v1.ComponentOrZero(i) + v2.ComponentOrZero(i);
            return new(result, copy: false);
        }

        /// <summary>Subtracts vectors, treating missing components as zero.</summary>
        public static Vector operator -(Vector v1, Vector v2)
        {
            double[] result = new double[Math.Max(v1.Length, v2.Length)];
            for (int i = 0; i < result.Length; i++)
                result[i] = v1.ComponentOrZero(i) - v2.ComponentOrZero(i);
            return new(result, copy: false);
        }

        /// <summary>Multiplies componentwise. Unmatched components are copied unchanged for compatibility.</summary>
        /// <remarks>This is not a dot or cross product; missing factors are treated as one.</remarks>
        public static Vector operator *(Vector v1, Vector v2)
        {
            double[] result = new double[Math.Max(v1.Length, v2.Length)];
            for (int i = 0; i < result.Length; i++)
                result[i] = (i < v1.Length ? v1[i] : 1) * (i < v2.Length ? v2[i] : 1);
            return new(result, copy: false);
        }

        /// <summary>Scales every component without changing the input.</summary>
        public static Vector operator *(Vector v1, double s)
        {
            double[] result = new double[v1.Length];
            for (int i = 0; i < result.Length; i++) result[i] = v1[i] * s;
            return new(result, copy: false);
        }

        /// <summary>Scales every component.</summary>
        public static Vector operator *(double s, Vector v1) => v1 * s;
        /// <summary>Negates all components without changing the input.</summary>
        public static Vector operator -(Vector v) => v * -1;
        /// <summary>Compares all components exactly, with null support.</summary>
        public static bool operator ==(Vector? v1, Vector? v2) => Equals(v1, v2);
        /// <summary>Compares all components exactly, with null support.</summary>
        public static bool operator !=(Vector? v1, Vector? v2) => !Equals(v1, v2);

        /// <summary>Returns a copy of all components; editing it does not change this vector.</summary>
        public double[] GetElements() => (double[])_elements.Clone();

        /// <summary>Returns an independent unit vector, using scaling to avoid overflow and underflow.</summary>
        /// <exception cref="InvalidOperationException">The vector is zero, empty, or has nonfinite components.</exception>
        public Vector GetNormalize()
        {
            double scale = ComponentScale();
            if (scale == 0 || !double.IsFinite(scale))
                throw new InvalidOperationException("A unit vector requires finite components and a nonzero magnitude.");
            double sum = 0;
            for (int i = 0; i < Length; i++)
            {
                double scaled = _elements[i] / scale;
                sum += scaled * scaled;
            }
            double norm = Math.Sqrt(sum);
            double[] result = new double[Length];
            for (int i = 0; i < Length; i++) result[i] = (_elements[i] / scale) / norm;
            return new(result, copy: false);
        }

        /// <summary>The right-handed cross product of two 3D vectors.</summary>
        /// <exception cref="ArgumentException">Either vector does not have exactly three components.</exception>
        public static Vector CrossProduct(Vector v1, Vector v2)
        {
            RequireThreeDimensions(v1, nameof(v1));
            RequireThreeDimensions(v2, nameof(v2));
            return new(v1.Y * v2.Z - v1.Z * v2.Y,
                       v1.Z * v2.X - v1.X * v2.Z,
                       v1.X * v2.Y - v1.Y * v2.X);
        }

        /// <summary>The right-handed cross product; both vectors must have three components.</summary>
        public Vector CrossProduct(Vector v) => CrossProduct(this, v);

        /// <summary>The dot product over all dimensions, treating missing components as zero.</summary>
        public static double DotProduct(Vector v1, Vector v2)
        {
            double result = 0;
            for (int i = 0; i < Math.Min(v1.Length, v2.Length); i++) result += v1[i] * v2[i];
            return result;
        }

        /// <summary>Copies all components to an N-by-1 column matrix.</summary>
        /// <exception cref="InvalidOperationException">An empty vector cannot form a matrix.</exception>
        public Matrix AsMatrix()
        {
            if (Length == 0) throw new InvalidOperationException("An empty vector cannot form a column matrix.");
            return new(_elements, Length, 1);
        }

        /// <summary>Formats all components using the current culture.</summary>
        public override string ToString() => $"<{string.Join(", ", _elements)}>";
        /// <summary>Compares all components exactly; numerical algorithms should use explicit tolerances.</summary>
        public override bool Equals(object? obj) => obj is Vector vector && _elements.SequenceEqual(vector._elements);
        /// <summary>Hashes all components. A vector used as a hash key must not be mutated.</summary>
        public override int GetHashCode()
        {
            HashCode hash = new();
            foreach (double element in _elements) hash.Add(element);
            return hash.ToHashCode();
        }

        private double ComponentOrZero(int i) => i < Length ? _elements[i] : 0;

        private double ComponentScale()
        {
            double scale = 0;
            foreach (double element in _elements) scale = Math.Max(scale, Math.Abs(element));
            return scale;
        }

        private double CalculateMagnitude()
        {
            double scale = ComponentScale();
            if (scale == 0 || !double.IsFinite(scale)) return scale;
            double sum = 0;
            foreach (double element in _elements)
            {
                double scaled = element / scale;
                sum += scaled * scaled;
            }
            return scale * Math.Sqrt(sum);
        }

        private static void RequireThreeDimensions(Vector vector, string parameterName)
        {
            ArgumentNullException.ThrowIfNull(vector, parameterName);
            if (vector.Length != 3) throw new ArgumentException("A cross product requires three-dimensional vectors.", parameterName);
        }
    }
}
