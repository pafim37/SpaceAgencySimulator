using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.OrbitalElements;

namespace Sas.Orbit.Service.Test
{
    public class PositionedOrbitTest
    {
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
            // TODO: Assert all 
            Assert.Equal(20001.7925, orbitElements.SemiMajorAxis!.Value, 4);
            Assert.Equal(0.4334, orbitElements.Eccentricity, 4);
            Assert.Equal(0.5270, orbitElements.Inclination, 4);
            Assert.Equal(0.7784, orbitElements.AscendingNode, 4);
            Assert.Equal(0.5355, orbitElements.ArgumentOfPeriapsis, 4);
            Assert.Equal(6.1235, orbitElements.TrueAnomaly, 4);
        }
    }
}
