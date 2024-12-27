using FluentAssertions;
using Sas.Body.Service.Models.Domain.Orbits.Helper;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Test
{
    public class OrbitTest
    {
        private const int Segments = 4;
        private const int SemiMajorAxis = 50;
        private const int SemiMinorAxis = 25;

        [Theory]
        [InlineData(0)]
        [InlineData(2 * Constants.PI)]
        public void GetEllipticOrbitPointsReturnsPoint(double rotation)
        {
            var points = GetEllipticOrbitPoints.GetPoints(
                semiMajorAxis: SemiMajorAxis,
                semiMinorAxis: SemiMinorAxis,
                center: Vector.Zero,
                rotation: rotation,
                segments: Segments
                );

            points.Should().HaveCount(Segments + 1);

            points[0].X.Should().Be(SemiMajorAxis);
            points[0].Y.Should().Be(0);
            points[0].Z.Should().Be(0);

            points[1].X.Should().Be(0);
            points[1].Y.Should().Be(SemiMinorAxis);
            points[1].Z.Should().Be(0);

            points[2].X.Should().Be(-SemiMajorAxis);
            points[2].Y.Should().Be(0);
            points[2].Z.Should().Be(0);

            points[3].X.Should().Be(0);
            points[3].Y.Should().Be(-SemiMinorAxis);
            points[3].Z.Should().Be(0);

            points[4].X.Should().Be(SemiMajorAxis);
            points[4].Y.Should().Be(0);
            points[4].Z.Should().Be(0);
        }

        [Fact]
        public void GetEllipticOrbitPointsTranslatePoint()
        {
            var points = GetEllipticOrbitPoints.GetPoints(
                semiMajorAxis: SemiMajorAxis,
                semiMinorAxis: SemiMinorAxis,
                center: 10 * Vector.Ones,
                rotation: 0,
                segments: Segments
                );

            points.Should().HaveCount(Segments + 1);

            points[0].X.Should().Be(SemiMajorAxis + 10);
            points[0].Y.Should().Be(10);
            points[0].Z.Should().Be(10);

            points[1].X.Should().Be(10);
            points[1].Y.Should().Be(SemiMinorAxis + 10);
            points[1].Z.Should().Be(10);

            points[2].X.Should().Be(-SemiMajorAxis + 10);
            points[2].Y.Should().Be(10);
            points[2].Z.Should().Be(10);

            points[3].X.Should().Be(10);
            points[3].Y.Should().Be(-SemiMinorAxis + 10);
            points[3].Z.Should().Be(10);

            points[4].X.Should().Be(SemiMajorAxis + 10);
            points[4].Y.Should().Be(10);
            points[4].Z.Should().Be(10);
        }

        [Fact]
        public void GetEllipticOrbitPointsRotatePoint()
        {
            var points = GetEllipticOrbitPoints.GetPoints(
                semiMajorAxis: SemiMajorAxis,
                semiMinorAxis: SemiMinorAxis,
                center: Vector.Zero,
                rotation: Math.PI / 2,
                segments: Segments
                );

            points.Should().HaveCount(Segments + 1);

            points[0].X.Should().Be(0);
            points[0].Y.Should().Be(SemiMajorAxis);
            points[0].Z.Should().Be(0);

            points[1].X.Should().Be(-SemiMinorAxis);
            points[1].Y.Should().Be(0);
            points[1].Z.Should().Be(0);

            points[2].X.Should().Be(0);
            points[2].Y.Should().Be(-SemiMajorAxis);
            points[2].Z.Should().Be(0);

            points[3].X.Should().Be(SemiMinorAxis);
            points[3].Y.Should().Be(0);
            points[3].Z.Should().Be(0);

            points[4].X.Should().Be(0);
            points[4].Y.Should().Be(SemiMajorAxis);
            points[4].Z.Should().Be(0);
        }
    }
}