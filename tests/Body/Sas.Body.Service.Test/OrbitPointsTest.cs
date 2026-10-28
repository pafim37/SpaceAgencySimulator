using FluentAssertions;
using Moq;
using Sas.Body.Service.Models.Domain.Orbits;
using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Body.Service.Models.Domain.Orbits.Points;

namespace Sas.Body.Service.Test;

public class OrbitPointsTest
{
    [Fact]
    public void EllipseUsesPerifocalToReferenceRotationAndClosesTheCurve()
    {
        var orbit = CreateOrbit(Math.PI / 2, Math.PI / 2, Math.PI / 2);
        List<Point> points = GetEllipticOrbitPoints.GetPoints(orbit, segments: 4);
        points.Should().HaveCount(5);
        double[][] expected = [[0, 0, 5], [0, -3, 0], [0, 0, -5], [0, 3, 0], [0, 0, 5]];
        for (int k = 0; k < points.Count; k++)
            AssertPoint(points[k], expected[k][0], expected[k][1], expected[k][2]);
    }

    [Fact]
    public void HyperbolaUsesTheSameRotationForEveryPoint()
    {
        var orbit = CreateOrbit(Math.PI / 2, Math.PI / 2, Math.PI / 2);
        List<Point> points = GetHyperbolicOrbitPoints.GetPoints(orbit, segments: 8);
        points.Should().HaveCount(9);
        for (int index = 0; index < points.Count; index++)
        {
            double t = 2 * Math.PI * (index - 4) / 8;
            // Three positive quarter-turns map (x,y,0) to (0,-y,x).
            AssertPoint(points[index], 0, -3 * Math.Sinh(t), 5 * Math.Cosh(t));
        }
    }

    [Fact]
    public void UndefinedOrbitalAnglesKeepTheExistingZeroAngleFallback()
    {
        var orbit = CreateOrbit(double.NaN, double.NaN, double.NaN);
        List<Point> ellipse = GetEllipticOrbitPoints.GetPoints(orbit, segments: 4);
        AssertPoint(ellipse[0], 5, 0, 0);
        AssertPoint(ellipse[1], 0, 3, 0);
        List<Point> hyperbola = GetHyperbolicOrbitPoints.GetPoints(orbit, segments: 8);
        AssertPoint(hyperbola[4], 5, 0, 0);
    }

    private static IPositionedOrbit CreateOrbit(double periapsis, double inclination, double node)
    {
        var description = new Mock<IOrbitDescription>();
        description.SetupGet(value => value.SemiMajorAxis).Returns(5);
        description.SetupGet(value => value.SemiMinorAxis).Returns(3);
        description.SetupGet(value => value.ArgumentOfPeriapsis).Returns(periapsis);
        description.SetupGet(value => value.Inclination).Returns(inclination);
        description.SetupGet(value => value.AscendingNode).Returns(node);
        var orbit = new Mock<IPositionedOrbit>();
        orbit.SetupGet(value => value.OrbitDescription).Returns(description.Object);
        return orbit.Object;
    }

    private static void AssertPoint(Point actual, double x, double y, double z)
    {
        actual.X.Should().BeApproximately(x, 1e-12 * Math.Max(1, Math.Abs(x)));
        actual.Y.Should().BeApproximately(y, 1e-12 * Math.Max(1, Math.Abs(y)));
        actual.Z.Should().BeApproximately(z, 1e-12 * Math.Max(1, Math.Abs(z)));
    }
}
