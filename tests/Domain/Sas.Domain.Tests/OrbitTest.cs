using Sas.Domain.Models.Orbits;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Converters;
using Sas.Mathematica.Service.Vectors;
using Xunit;

namespace Sas.Domain.Tests
{
    public class OrbitTest
    {
        [Fact]
        public void OrbitReturnsOrbitalElementsForEarthInApoapsis()
        {
            // Arrange
            Vector position = new Vector(Constants.EarthApoapsis, 0, 0);
            Vector velocity = new Vector(0, Constants.EarthMinVelocity, 0);
            double u = Constants.G * (Constants.SolarMass + Constants.EarthMass);

            // Act
            Orbit orbit = OrbitFactory.CalculateOrbit(position, velocity, u);

            // Assert
            Assert.Equal(149588585873.2210, orbit.SemiMajorAxis!.Value, 4);
            Assert.Equal(0.01677, orbit.Eccentricity, 4);
            Assert.Equal(3.1416, orbit.MeanAnomaly, 4);
            Assert.Equal(double.NaN, orbit.ArgumentOfPeriapsis, 4);
            Assert.Equal(0, orbit.Inclination, 4);
            Assert.Equal(double.NaN, orbit.AscendingNode, 4);
            Assert.Equal(3.1416, orbit.TrueAnomaly, 4);
            Assert.Equal(3.1416, orbit.EccentricAnomaly, 4);
            Assert.Equal(31555285.4183, orbit.Period!.Value, 4);
        }

        [Fact]
        public void OrbitReturnsOrbitalElements()
        {
            // Arrange
            Vector position = new Vector(5000, 10000, 2100);
            Vector velocity = new Vector(-5.922, 1.926, 3.246);
            double u = 398600;

            // Act
            Orbit orbit = OrbitFactory.CalculateOrbit(position, velocity, u);

            // Assert
            Assert.Equal(19198.3565, orbit.SemiMajorAxis!.Value, 4);
            Assert.Equal(0.4095, orbit.Eccentricity, 4);
            Assert.Equal(0.5302, orbit.Inclination, 4);
            Assert.Equal(0.7810, orbit.AscendingNode, 4);
            Assert.Equal(0.5261, orbit.ArgumentOfPeriapsis, 4);
            Assert.Equal(6.1307, orbit.TrueAnomaly, 4);
        }
    }
}
