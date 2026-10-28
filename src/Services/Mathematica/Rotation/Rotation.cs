using Sas.Mathematica.Service.Matrices;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Service.Rotation
{
    /// <summary>Active, right-handed rotations of 3D column vectors. All angles are in radians.</summary>
    public static class Rotation
    {
        /// <summary>Rotates a vector about a nonzero axis; inputs are not modified.</summary>
        /// <param name="vectorToRotate">A finite three-dimensional vector.</param>
        /// <param name="axis">A finite nonzero 3D axis; normalized internally.</param>
        /// <param name="angle">The finite rotation angle in radians, following the right-hand rule.</param>
        /// <param name="inverseRotation">If true, negates the angle.</param>
        /// <exception cref="ArgumentException">A vector is not finite 3D data or the axis is zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The angle is not finite.</exception>
        public static Vector Rotate(Vector vectorToRotate, Vector axis, double angle, bool inverseRotation = false)
        {
            ValidateVector(vectorToRotate, nameof(vectorToRotate));
            ValidateVector(axis, nameof(axis));
            ValidateAngle(angle, nameof(angle));
            if (axis.X == 0 && axis.Y == 0 && axis.Z == 0)
                throw new ArgumentException("The rotation axis must be nonzero.", nameof(axis));

            Vector unit = axis.GetNormalize();
            double cos = Math.Cos(angle);
            double sin = Math.Sin(inverseRotation ? -angle : angle);
            double projection = Vector.DotProduct(unit, vectorToRotate) * (1 - cos);
            // Rodrigues' formula avoids allocating intermediate matrices.
            return new Vector(
                vectorToRotate.X * cos + (unit.Y * vectorToRotate.Z - unit.Z * vectorToRotate.Y) * sin + unit.X * projection,
                vectorToRotate.Y * cos + (unit.Z * vectorToRotate.X - unit.X * vectorToRotate.Z) * sin + unit.Y * projection,
                vectorToRotate.Z * cos + (unit.X * vectorToRotate.Y - unit.Y * vectorToRotate.X) * sin + unit.Z * projection);
        }

        /// <summary>Maps perifocal column vectors into the reference frame using Rz(Ω) * Rx(i) * Rz(ω).</summary>
        /// <param name="argumentOfPeriapsis">ω in radians.</param>
        /// <param name="inclination">i in radians.</param>
        /// <param name="ascendingNode">Ω in radians.</param>
        /// <exception cref="ArgumentOutOfRangeException">An angle is not finite.</exception>
        public static Matrix RotationMatrix3D(double argumentOfPeriapsis, double inclination, double ascendingNode)
        {
            ValidateAngle(argumentOfPeriapsis, nameof(argumentOfPeriapsis));
            ValidateAngle(inclination, nameof(inclination));
            ValidateAngle(ascendingNode, nameof(ascendingNode));
            double nodeCos = Math.Cos(ascendingNode);
            double nodeSin = Math.Sin(ascendingNode);
            double inclinationCos = Math.Cos(inclination);
            double inclinationSin = Math.Sin(inclination);
            double periapsisCos = Math.Cos(argumentOfPeriapsis);
            double periapsisSin = Math.Sin(argumentOfPeriapsis);
            return new Matrix([
                nodeCos * periapsisCos - nodeSin * inclinationCos * periapsisSin,
                -nodeCos * periapsisSin - nodeSin * inclinationCos * periapsisCos,
                nodeSin * inclinationSin,
                nodeSin * periapsisCos + nodeCos * inclinationCos * periapsisSin,
                -nodeSin * periapsisSin + nodeCos * inclinationCos * periapsisCos,
                -nodeCos * inclinationSin,
                inclinationSin * periapsisSin,
                inclinationSin * periapsisCos,
                inclinationCos
            ], 3, 3);
        }

        private static void ValidateVector(Vector vector, string parameterName)
        {
            ArgumentNullException.ThrowIfNull(vector, parameterName);
            if (vector.Length != 3 || !double.IsFinite(vector.X) || !double.IsFinite(vector.Y) || !double.IsFinite(vector.Z))
                throw new ArgumentException("Expected a three-dimensional vector with finite components.", parameterName);
        }

        private static void ValidateAngle(double angle, string parameterName)
        {
            if (!double.IsFinite(angle)) throw new ArgumentOutOfRangeException(parameterName, "Angles must be finite radians.");
        }
    }
}
