using FluentAssertions;
using Sas.Domain.Models.Bodies;
using Sas.Mathematica.Service.Vectors;
using Xunit;

namespace Sas.Domain.Tests
{
    public class BodyTest
    {
        [Fact]
        public void BodyShouldAssignProperties()
        {
            // Arrange
            string name = "Planet";
            double mass = 10;
            Vector position = Vector.Ones;
            Vector velocity = Vector.Ones;

            // Act
            Body body = new(name, mass, position, velocity);

            // Assert
            body.Name.Should().Be(name);
            body.Mass.Should().Be(mass);
            body.Position.Should().Be(position);
            body.Velocity.Should().Be(velocity);
            body.Radius.Should().Be(0);
        }

        [Fact]
        public void BodyShouldAssignPropertiesWithRadius()
        {
            // Arrange
            string name = "Planet";
            double mass = 10;
            Vector position = Vector.Ones;
            Vector velocity = Vector.Ones;
            double radius = 10;

            // Act
            Body body = new(name, mass, position, velocity, radius);

            // Assert
            body.Name.Should().Be(name);
            body.Mass.Should().Be(mass);
            body.Position.Should().Be(position);
            body.Velocity.Should().Be(velocity);
            body.Radius.Should().Be(radius);
        }

        [Fact]
        public void BodyShouldThrowsExceptionWhenNegativeMass()
        {
            // Arrange
            Vector position = Vector.Zero;
            Vector velocity = Vector.Zero;
            double negativeMass = -1;

            Action action = () =>
            {
                new Body("Planet", negativeMass, position, velocity);
            };

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void BodyShouldThrowsExceptionWhenNegativeRadius()
        {
            // Arrange
            Vector position = Vector.Zero;
            Vector velocity = Vector.Zero;
            double negativeRadius = -1;

            Action action = () =>
            {
                new Body("Planet", 0, position, velocity, negativeRadius);
            };

            // Act & Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ParameterlessConstructorCreatesBody()
        {
            // Arrange & Act
            var body = new Body();

            // Assert
            body.Should().NotBeNull();
        }
    }
}