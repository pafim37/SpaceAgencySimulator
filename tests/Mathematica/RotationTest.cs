using FluentAssertions;
using Sas.Mathematica.Service.Converters;
using Sas.Mathematica.Service.Rotation;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Tests
{
    public class RotationTest
    {
        private const int Precision = 10;
        public static IEnumerable<object[]> RotationOXAroundOZTestData()
        {
            yield return new object[]
            {
                ConvertAngle.DegToRad(45),
                new Vector([Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0]),
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(90),
                Vector.Oy,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(180),
                -Vector.Ox,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(270),
                -Vector.Oy,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(360),
                Vector.Ox,
            };
        }
        public static IEnumerable<object[]> RotationOYAroundOZTestData()
        {
            yield return new object[]
            {
                ConvertAngle.DegToRad(45),
                new Vector([-Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0]),
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(90),
                -Vector.Ox,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(180),
                -Vector.Oy,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(270),
                Vector.Ox,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(360),
                Vector.Oy,
            };
        }

        public static IEnumerable<object[]> RotationOXAroundOYTestData()
        {
            yield return new object[]
            {
                ConvertAngle.DegToRad(45),
                new Vector([Math.Sqrt(2) / 2, 0, -Math.Sqrt(2) / 2]),
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(90),
                -Vector.Oz,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(180),
                -Vector.Ox,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(270),
                Vector.Oz,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(360),
                Vector.Ox,
            };
        }

        public static IEnumerable<object[]> RotationOZAroundOYTestData()
        {
            yield return new object[]
            {
                ConvertAngle.DegToRad(45),
                new Vector([Math.Sqrt(2) / 2, 0, Math.Sqrt(2) / 2]),
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(90),
                Vector.Ox,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(180),
                -Vector.Oz,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(270),
                -Vector.Ox,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(360),
                Vector.Oz,
            };
        }

        public static IEnumerable<object[]> RotationOYAroundOXTestData()
        {
            yield return new object[]
            {
                ConvertAngle.DegToRad(45),
                new Vector([0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2]),
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(90),
                Vector.Oz,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(180),
                -Vector.Oy,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(270),
                -Vector.Oz,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(360),
                Vector.Oy,
            };
        }

        public static IEnumerable<object[]> RotationOZAroundOXTestData()
        {
            yield return new object[]
            {
                ConvertAngle.DegToRad(45),
                new Vector([0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2]),
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(90),
                -Vector.Oy,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(180),
                -Vector.Oz,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(270),
                Vector.Oy,
            };
            yield return new object[]
            {
                ConvertAngle.DegToRad(360),
                Vector.Oz,
            };
        }

        [Theory]
        [MemberData(nameof(RotationOXAroundOZTestData))]
        public void RotationRotateVectorOXAroundOZ(double angle, Vector expected)
        {
            Vector vectorToRotate = Vector.Ox;
            Vector axis = Vector.Oz;
            Vector result = Rotation.Rotate(vectorToRotate, axis, angle);
            Assert(expected, result);
        }


        [Theory]
        [MemberData(nameof(RotationOYAroundOZTestData))]
        public void RotationRotateVectorOYAroundOZ(double angle, Vector expected)
        {
            Vector vectorToRotate = Vector.Oy;
            Vector axis = Vector.Oz;
            Vector result = Rotation.Rotate(vectorToRotate, axis, angle);
            Assert(expected, result);
        }

        [Theory]
        [MemberData(nameof(RotationOXAroundOYTestData))]
        public void RotationRotateVectorOXAroundOY(double angle, Vector expected)
        {
            Vector vectorToRotate = Vector.Ox;
            Vector axis = Vector.Oy;
            Vector result = Rotation.Rotate(vectorToRotate, axis, angle);
            Assert(expected, result);
        }

        [Theory]
        [MemberData(nameof(RotationOZAroundOYTestData))]
        public void RotationRotateVectorOZAroundOY(double angle, Vector expected)
        {
            Vector vectorToRotate = Vector.Oz;
            Vector axis = Vector.Oy;
            Vector result = Rotation.Rotate(vectorToRotate, axis, angle);
            Assert(expected, result);
        }

        [Theory]
        [MemberData(nameof(RotationOYAroundOXTestData))]
        public void RotationRotateVectorOYAroundOX(double angle, Vector expected)
        {
            Vector vectorToRotate = Vector.Oy;
            Vector axis = Vector.Ox;
            Vector result = Rotation.Rotate(vectorToRotate, axis, angle);
            Assert(expected, result);
        }

        [Theory]
        [MemberData(nameof(RotationOZAroundOXTestData))]
        public void RotationRotateVectorOZAroundOX(double angle, Vector expected)
        {
            Vector vectorToRotate = Vector.Oz;
            Vector axis = Vector.Ox;
            Vector result = Rotation.Rotate(vectorToRotate, axis, angle);
            Assert(expected, result);
        }

        [Fact]
        public void RotationRotateVectorThrowsExceptionWhenInvalidVectorToRotate()
        {
            Vector invalidVectorToRotate = new([1,1,1,1]);
            Action comparison = () => Rotation.Rotate(invalidVectorToRotate, Vector.Ox, 1);
            comparison.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RotationRotateVectorThrowsExceptionWhenInvalidAxis()
        {
            Vector invalidAxis = new([1, 1, 1, 1]);
            Action comparison = () => Rotation.Rotate(Vector.Ox, invalidAxis, 1);
            comparison.Should().Throw<ArgumentException>();
        }

        private static void Assert(Vector expected, Vector result)
        {
            result.X.Should().Be(expected.X);
            result.Y.Should().Be(expected.Y);
            result.Z.Should().Be(expected.Z);
        }
    }
}
