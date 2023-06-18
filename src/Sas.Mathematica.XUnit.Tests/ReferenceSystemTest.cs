using FluentAssertions;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Converters;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.XUnit.Tests
{
    public class ReferenceSystemTest
    {
        public static IEnumerable<object[]> ReferenceSystemPhiDataTest =>
            new List<object[]>
            {
                new object[] { new Vector(1, 0), ConvertAngle.DegToRad(0) }, 
                new object[] { new Vector(1, 1), ConvertAngle.DegToRad(45) },
                new object[] { new Vector(0, 1), ConvertAngle.DegToRad(90) },
                new object[] { new Vector(-1, 1), ConvertAngle.DegToRad(90 + 45) },
                new object[] { new Vector(-1, 0), ConvertAngle.DegToRad(180) },
                new object[] { new Vector(-1, -1), ConvertAngle.DegToRad(180 + 45) },
                new object[] { new Vector(0, -1), ConvertAngle.DegToRad(180 + 90) },
                new object[] { new Vector(1, -1), ConvertAngle.DegToRad(180 + 90 + 45) },
            };

        public static IEnumerable<object[]> ReferenceSystemThetaDataTest =>
            new List<object[]>
            {
                new object[] { new Vector(1, 1, 0), ConvertAngle.DegToRad(0) },
                new object[] { new Vector(1, 1, 1), ConvertAngle.DegToRad(45) },
                new object[] { new Vector(0, 0, 1), ConvertAngle.DegToRad(90) },
                new object[] { new Vector(-1, -1, 1), ConvertAngle.DegToRad(45) },
                new object[] { new Vector(-1, -1, 0), ConvertAngle.DegToRad(0) },

                new object[] { new Vector(1, 1, -1), ConvertAngle.DegToRad(-45) },
                new object[] { new Vector(0, 0, -1), ConvertAngle.DegToRad(-90) },
                new object[] { new Vector(-1, -1, -1), ConvertAngle.DegToRad(-45) },
                new object[] { new Vector(-1, -1, -0), ConvertAngle.DegToRad(-0) },
            };

        [Fact]
        public void ReferenceSystemReturnsPointCoordinates()
        {
            // Assign
            Vector point = Vector.Ones;

            // Act
            ReferenceSystem referenceSystem = new(point);

            // Assert
            referenceSystem.X.Should().Be(1);
            referenceSystem.Y.Should().Be(1);
            referenceSystem.Z.Should().Be(1);
            referenceSystem.R.Should().Be(Math.Sqrt(3));
            referenceSystem.Phi.Should().Be(ConvertAngle.DegToRad(45));
            referenceSystem.Th.Should().Be(ConvertAngle.DegToRad(45));
        }

        [Fact]
        public void ReferenceSystemReturnsPointCoordinatesRelativeToOrigin()
        {
            // Assign
            Vector origin = Vector.Ones;
            Vector point = Vector.Zero;

            // Act
            ReferenceSystem referenceSystem = new(origin, point);

            // Assert
            referenceSystem.X.Should().Be(-1);
            referenceSystem.Y.Should().Be(-1);
            referenceSystem.Z.Should().Be(-1);
            referenceSystem.R.Should().Be(Math.Sqrt(3));
            referenceSystem.Phi.Should().Be(ConvertAngle.DegToRad(225));
            referenceSystem.Th.Should().Be(ConvertAngle.DegToRad(-45));
            referenceSystem.PhiAsDeg.Should().Be(225);
            referenceSystem.ThAsDeg.Should().Be(-45);
        }

        [Theory]
        [MemberData(nameof(ReferenceSystemPhiDataTest))]
        public void ReferenceSystemReturnsCorrectPhi(Vector point, double phi)
        {
            // Assign
            Vector origin = Vector.Zero;

            // Act
            ReferenceSystem referenceSystem = new(origin, point);

            // Assert
            referenceSystem.Phi.Should().Be(phi);
        }

        [Theory]
        [MemberData(nameof(ReferenceSystemThetaDataTest))]
        public void ReferenceSystemReturnsCorrectTheta(
            Vector point, double th)
        {
            // Assign
            Vector origin = Vector.Zero;

            // Act
            ReferenceSystem referenceSystem = new(origin, point);

            // Assert
            referenceSystem.Th.Should().Be(th);
        }
    }
}
