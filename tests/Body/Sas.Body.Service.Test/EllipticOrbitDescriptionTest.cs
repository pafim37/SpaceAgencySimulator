using Sas.Body.Business.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Test;

public class EllipticOrbitDescriptionTest
{
    [Theory]
    [InlineData(0.5, 0.7, 0.8, 1.2)]
    [InlineData(0.5, 4.0, 4.2, 4.8)]
    [InlineData(2.4, 0.7, 4.2, 1.2)]
    [InlineData(2.4, 4.0, 0.8, 4.8)]
    public void RecoversAnglesFromRotatedEllipticState(
        double inclination, double ascendingNode, double periapsis, double trueAnomaly)
    {
        // p = 3, e = 0.5, mu = 3 gives a = 4 and velocity scale = 1.
        double radius = 3 / (1 + 0.5 * Math.Cos(trueAnomaly));
        Vector position = Rotate(radius * Math.Cos(trueAnomaly), radius * Math.Sin(trueAnomaly),
            periapsis, inclination, ascendingNode);
        Vector velocity = Rotate(-Math.Sin(trueAnomaly), 0.5 + Math.Cos(trueAnomaly),
            periapsis, inclination, ascendingNode);

        var orbit = new EllipticOrbitDescription(position, velocity, 3);

        Assert.Equal(inclination, orbit.Inclination, 10);
        Assert.Equal(ascendingNode, orbit.AscendingNode, 10);
        Assert.Equal(periapsis, orbit.ArgumentOfPeriapsis, 10);
        Assert.Equal(trueAnomaly, orbit.TrueAnomaly, 10);
        Assert.Equal(0.5, orbit.Eccentricity, 10);
        Assert.Equal(4, orbit.SemiMajorAxis!.Value, 10);
        Assert.Equal(Math.Sqrt(12), orbit.SemiMinorAxis!.Value, 10);
    }

    [Theory]
    [InlineData(2, 1.5, 0)]
    [InlineData(6, 0.5, Math.PI)]
    public void ReturnsTrueAnomalyAtApsides(double radius, double speed, double expectedAnomaly)
    {
        var orbit = new EllipticOrbitDescription(new Vector(radius, 0, 0), new Vector(0, speed, 0), 3);

        Assert.Equal(expectedAnomaly, orbit.TrueAnomaly, 10);
        Assert.Equal(0, orbit.Inclination, 10);
        Assert.True(double.IsNaN(orbit.AscendingNode));
    }

    [Theory]
    [InlineData(0.7)]
    [InlineData(-0.7)]
    public void UsesEccentricityDirectionForEquatorialPeriapsis(double periapsis)
    {
        Vector position = Rotate(2, 0, periapsis, 0, 0);
        Vector velocity = Rotate(0, 1.5, periapsis, 0, 0);

        var orbit = new EllipticOrbitDescription(position, velocity, 3);

        Assert.Equal(periapsis, orbit.ArgumentOfPeriapsis, 10);
        Assert.True(double.IsNaN(orbit.AscendingNode));
    }

    [Fact]
    public void RadialMotionHasUndefinedInclinationAndAscendingNode()
    {
        var orbit = new EllipticOrbitDescription(new Vector(2, 0, 0), new Vector(0.5, 0, 0), 3);

        Assert.True(double.IsNaN(orbit.Inclination));
        Assert.True(double.IsNaN(orbit.AscendingNode));
    }

    private static Vector Rotate(double x, double y, double periapsis, double inclination, double ascendingNode)
    {
        double rotatedX = x * Math.Cos(periapsis) - y * Math.Sin(periapsis);
        double rotatedY = x * Math.Sin(periapsis) + y * Math.Cos(periapsis);
        return new Vector(
            rotatedX * Math.Cos(ascendingNode) - rotatedY * Math.Cos(inclination) * Math.Sin(ascendingNode),
            rotatedX * Math.Sin(ascendingNode) + rotatedY * Math.Cos(inclination) * Math.Cos(ascendingNode),
            rotatedY * Math.Sin(inclination));
    }
}
