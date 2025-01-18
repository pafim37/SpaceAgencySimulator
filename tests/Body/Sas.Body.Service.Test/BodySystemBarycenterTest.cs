using FluentAssertions;
using Sas.Body.Service.Models.Domain;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Test
{
    public class BodySystemBarycenterTest
    {
        [Fact]
        public void BodySystemReturnsBarycentrum()
        {
            // Arrange
            BodyDomain body1 = new("B1", 10, new Vector(50), new Vector(0, 1));
            BodyDomain body2 = new("B2", 10, new Vector(0), new Vector(0, -1));

            List<BodyDomain> bodies = [body1, body2];

            // Act
            BodySystem bodySystem = new(bodies);
            bodySystem.UpdateBodySystem();

            // Assert
            BodyDomain? barycentrum = bodySystem.Barycentrum;
            barycentrum.Should().NotBeNull();
            barycentrum!.Name.Should().Be("Barycentrum");
            barycentrum.Mass.Should().Be(20);
            barycentrum.Radius.Should().Be(0);
            barycentrum.Position.X.Should().Be(25);
            barycentrum.Position.Y.Should().Be(0);
            barycentrum.Position.Z.Should().Be(0);
            barycentrum.Velocity.Should().Be(Vector.Zero);
        }

        [Fact]
        public void BodySystemReturnsBarycentrumAsNullWhenNoBodies()
        {
            BodySystem bodySystem = new([]);
            bodySystem.Barycentrum.Should().BeNull();
        }

        [Fact]
        public void BodySystemReturnsBarycentrumAsBodyWhenOneBody()
        {
            BodyDomain body1 = new("B1", 10, Vector.Ones, Vector.Ones);
            BodySystem bodySystem = new([body1]);
            bodySystem.UpdateBodySystem();
            BodyDomain? barycentrum = bodySystem.Barycentrum;
            barycentrum.Should().NotBeNull();
            barycentrum!.Name.Should().Be("Barycentrum");
            barycentrum.Mass.Should().Be(body1.Mass);
            barycentrum.Position.Should().Be(body1.Position);
            barycentrum.Velocity.Should().Be(body1.Velocity);
        }

        [Fact]
        public void CalibrateBodyForBarycentrum()
        {
            BodyDomain body1 = new("B1", 10, Vector.Ones, Vector.Ones);
            BodySystem bodySystem = new([body1]);
            bodySystem.UpdateBodySystem();
            bodySystem.CalibrateBarycenterToZero();
            BodyDomain? barycenter = bodySystem.Barycentrum;
            body1.Position.Should().Be(Vector.Zero);
            body1.Velocity.Should().Be(Vector.Zero);
            barycenter.Should().NotBeNull();
            barycenter!.Position.Should().Be(Vector.Zero);
            barycenter.Velocity.Should().Be(Vector.Zero);
        }

        [Fact]
        public void CalibrateBodiesVelocityForBarycentrum()
        {
            // Arrange
            BodyDomain body1 = new("B1", 100, new Vector(0), new Vector(0, 1));
            BodyDomain body2 = new("B2", 1, new Vector(50), new Vector(0, 2));
            BodySystem bodySystem = new([body1, body2]);
            bodySystem.UpdateBodySystem();

            // Act
            bodySystem.CalibrateBarycenterToZero();

            // Assert
            BodyDomain? barycenter = bodySystem.Barycentrum;
            body1.Velocity.Should().Be(Vector.Zero);
            body2.Velocity.Should().Be(new Vector(0, 1));
            barycenter!.Velocity.Should().Be(Vector.Zero);
        }
    }
}
