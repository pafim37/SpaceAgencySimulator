using Sas.Domain.Bodies;
using Sas.Domain.Models.Bodies;
using Sas.Domain.Models.Orbits.Primitives;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using Xunit;

namespace Sas.Domain.Tests
{
    public class BodyTest
    {
        [Fact]
        public void RadiusOfTheBodyIsZeroWhenBodyIsBeingCreatedWithoutRadius()
        {
            // Arrange
            Vector position = Vector.Zero;
            Vector velocity = Vector.Zero;

            // Act
            Body body = new Body("Planet", 10, position, velocity);

            // Assert
            Assert.Equal(0, body.Radius);
        }

        [Fact]
        public void BodyShouldThrowExceptionWhenNonNegativeMass()
        {
            // Arrange
            Vector position = Vector.Zero;
            Vector velocity = Vector.Zero;
            double negativeMass = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Body("Planet", negativeMass, position, velocity));
        }

        [Fact]
        public void BodyShouldThrowExceptionWhenNonNegativeRadius()
        {
            // Arrange
            Vector position = Vector.Zero;
            Vector velocity = Vector.Zero;
            double negativeRadius = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Body("Planet", 0, position, velocity, negativeRadius));
        }

        [Fact]
        public void BodyShouldAssignProperRadius()
        {
            // Arrange
            Vector position = Vector.Zero;
            Vector velocity = Vector.Zero;
            double radius = 37.42;

            // Act
            Body body = new Body("Planet", 0, position, velocity, radius);

            // Assert
            Assert.Equal(radius, body.Radius);
        }


        // TODO: Improve that test. Consider what precision should be for Circular Orbit
        [Fact]
        public void BodySystemReturnsCircularOrbit()
        {
            // Arrange
            double M = 10000000000; // TODO: is it heavy?
            double rx = 10;
            Vector smallVelocity = new Vector(0, Math.Sqrt(Constants.G * M / rx), 0);
            Body smallBody = new Body("Small Body", 10, new Vector(rx, 0, 0), smallVelocity);
            Body bigBody = new Body("Big body", M, Vector.Zero, Vector.Zero);
            List<Body> bodies = new List<Body>() { bigBody, smallBody };

            // Act
            BodySystem bodySystem = new BodySystem(bodies);

            // Assert
            Assert.Equal("Barycentrum", bodySystem.Barycentrum.Name);
            Assert.Equal(10000000010, bodySystem.Barycentrum.Mass);
            Assert.Equal(Vector.Zero, bodySystem.Barycentrum.Position);
            Assert.Equal(Vector.Zero, bodySystem.Barycentrum.Velocity);
            Assert.Equal(0, bodySystem.Barycentrum.Radius);
            Assert.Equal(0.66743, bodySystem.U, 5);

            Assert.Equal(OrbitType.Circular, bodySystem.OrbitsDescription.First().Orbit.OrbitType);


        }

    }
}