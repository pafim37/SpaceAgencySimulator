using Sas.Domain.Models.Bodies;
using Sas.Domain.Models.Orbits.Primitives;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using Xunit;

namespace Sas.Domain.Tests
{
    public class BodySystemTest
    {
        [Fact]
        public void BodySystemReturnsCircularOrbit()
        {
            // Arrange
            double M = double.MaxValue;
            double rx = 1000;
            Vector smallVelocity = new Vector(0, Math.Sqrt(Constants.G * M / rx), 0);
            Body smallBody = new Body("Small Body", 1, new Vector(rx, 0, 0), smallVelocity);
            Body bigBody = new Body("Big body", M, Vector.Zero, Vector.Zero);
            List<Body> bodies = new List<Body>() { bigBody, smallBody };

            // Act
            BodySystem bodySystem = new BodySystem(bodies);

            // Assert
            Assert.Equal(OrbitType.Circular, bodySystem.OrbitsDescription.First().Orbit!.OrbitType);
        }
    }
}
