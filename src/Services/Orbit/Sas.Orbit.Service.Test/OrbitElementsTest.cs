using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.OrbitalElements;
using Sas.Orbit.Service.Orbits.Primitives;

namespace Sas.Orbit.Service.Test
{
    public class OrbitElementsTest
    {
        [Fact]
        public void OrbitReturnsOrbitalElementsForEarthInApoapsis()
        {
            // Arrange
            Vector position = new(Constants.EarthApoapsis, 0, 0);
            Vector velocity = new(0, Constants.EarthMinVelocity, 0);
            double u = Constants.G * (Constants.SolarMass + Constants.EarthMass);

            // Act
            OrbitElements orbit = OrbitElementsFactory.GetOrbitElements(position, velocity, u);

            // Assert
            Assert.Equal(149588585873.2210, orbit.SemiMajorAxis!.Value, 4);
            Assert.Equal(0.01677, orbit.Eccentricity, 4);
            Assert.Equal(3.1416, orbit.MeanAnomaly, 4);
            Assert.Equal(3.141592, orbit.ArgumentOfPeriapsis, 4);
            Assert.Equal(0, orbit.Inclination, 4);
            Assert.Equal(double.NaN, orbit.AscendingNode, 4);
            Assert.Equal(3.1416, orbit.TrueAnomaly, 4);
            Assert.Equal(3.1416, orbit.EccentricAnomaly, 4);
            Assert.Equal(31555285.4183, orbit.Period!.Value, 4);
        }

        [Fact]
        public void OrbitElementsFactoryReturnsOrbitalElementsAndPoints()
        {
            // Arrange
            Vector position = new(5000, 10000, 2100);
            Vector velocity = new(-5.992, 1.926, 3.246);
            double u = 398600;

            // Act
            OrbitElements orbitElements = OrbitElementsFactory.GetOrbitElements(position, velocity, u);

            // Assert
            Assert.Equal(OrbitType.Elliptic, orbitElements.OrbitType);
            Assert.Equal(20001.7925, orbitElements.SemiMajorAxis!.Value, 4);
            Assert.Equal(18025.2071, orbitElements.SemiMinorAxis!.Value, 4);
            Assert.Equal(0.4334, orbitElements.Eccentricity, 4);
            Assert.Equal(0.5270, orbitElements.Inclination, 4);
            Assert.Equal(0.7784, orbitElements.AscendingNode, 4);
            Assert.Equal(0.5355, orbitElements.ArgumentOfPeriapsis, 4);
            Assert.Equal(6.1235, orbitElements.TrueAnomaly, 4);
            Assert.Equal(0.1005, orbitElements.EccentricAnomaly, 4);
            Assert.Equal(0.0570, orbitElements.MeanAnomaly, 4);
            Assert.Equal(28152.3465, orbitElements.Period!.Value, 4);
            Assert.Null(orbitElements.Radius);
            Assert.Equal(398600, orbitElements.GravitationalParameter, 4);
            Assert.Equal(40467.7087, orbitElements.FirstNode, 4);
            Assert.Equal(0.0002, orbitElements.MeanMotion, 4);
            Assert.Equal(0.1312, orbitElements.EccentricityVector.X, 4);
            Assert.Equal(0.3979, orbitElements.EccentricityVector.Y, 4);
            Assert.Equal(0.1112, orbitElements.EccentricityVector.Z, 4);
        }
    }
}
