using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits;
using Sas.Orbit.Service.Orbits.OrbitalElements;

namespace Sas.Orbit.Service.Test
{
    public class PositionedOrbitTest
    {
        [Fact]
        public void OrbitReturnsOrbitalElements()
        {
            // Arrange
            string orbitName = "Test Orbit";
            Vector position = new(5000, 10000, 2100);
            Vector velocity = new(-5.922, 1.926, 3.246);
            double u = 398600;

            // Act
            OrbitElements orbitElements = OrbitElementsFactory.GetOrbitElements(position, velocity, u);
            PositionedOrbit positionedOrbit = new(orbitName, gravitationalSourcePosition: Vector.Zero, orbitElements);

            // Assert
            // Orbit name
            Assert.Equal(orbitName, positionedOrbit.Name);
            // Orbit elements
            Assert.Equal(19198.3565, orbitElements.SemiMajorAxis!.Value, 4);
            Assert.Equal(0.4095, positionedOrbit.OrbitElements.Eccentricity, 4);
            Assert.Equal(0.5302, positionedOrbit.OrbitElements.Inclination, 4);
            Assert.Equal(0.7810, positionedOrbit.OrbitElements.AscendingNode, 4);
            Assert.Equal(0.5261, positionedOrbit.OrbitElements.ArgumentOfPeriapsis, 4);
            Assert.Equal(6.1307, positionedOrbit.OrbitElements.TrueAnomaly, 4);

            // Points
            Assert.Equal(51, positionedOrbit.Points.Count);
            Assert.Equal(positionedOrbit.Points[0].X, positionedOrbit.Points[50].X, 4);
            Assert.Equal(positionedOrbit.Points[0].Y, positionedOrbit.Points[50].Y, 4);
            Assert.Equal(positionedOrbit.Points[0].Z, positionedOrbit.Points[50].Z, 4);
        }
    }
}
