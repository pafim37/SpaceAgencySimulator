using FluentAssertions;
using Sas.Body.Service.Models.Domain;
using Sas.Body.Service.Models.Domain.Orbits.Primitives;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Test
{
    public class BodySystemTest
    {
        [Fact]
        public void BodySystemReturnsBarycentrum()
        {
            BodyDomain body1 = new BodyDomain("B1", 10, new Vector(50), new Vector(0, 1));
            BodyDomain body2 = new BodyDomain("B2", 10, new Vector(0), new Vector(0, -1));
            
            List<BodyDomain> bodies = [body1, body2];
            BodySystem bodySystem = new BodySystem(bodies);

            BodyDomain barycentrum = bodySystem.Barycentrum;

            barycentrum.Should().NotBeNull();
            barycentrum.Name.Should().Be("Barycentrum");
            barycentrum.Mass.Should().Be(20);
            barycentrum.Radius.Should().Be(0);
            barycentrum.Position.X.Should().Be(25);
            barycentrum.Position.Y.Should().Be(0);
            barycentrum.Position.Z.Should().Be(0);
            barycentrum.Velocity.Should().Be(Vector.Zero);
        }

        [Fact]
        public void BodySystemReturnsCircularOrbit()
        {
            // Arrange
            double M = double.MaxValue;
            double rx = 1000;
            Vector smallVelocity = new(0, Math.Sqrt(Constants.G * M / rx), 0);
            BodyDomain smallBody = new("Small Body", 1, new Vector(rx, 0, 0), smallVelocity);
            BodyDomain bigBody = new("Big body", M, Vector.Zero, Vector.Zero);
            List<BodyDomain> bodies = [bigBody, smallBody];

            // Act
            BodySystem bodySystem = new(bodies);

            // Assert
            Assert.Equal(OrbitType.Circular, bodySystem.Orbits.First()!.OrbitType);
        }
    }
}
