using Sas.Domain.Bodies;
using Sas.Domain.Models.Bodies;
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
            Assert.Equal(radius, body.Radius);
        }

    }
}