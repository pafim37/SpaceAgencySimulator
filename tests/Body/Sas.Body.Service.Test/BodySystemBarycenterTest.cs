using FluentAssertions;
using Sas.Mathematica.Service.Vectors;
using BodySystemsModel = Sas.BodySystem.Service.BodySystems.BodySystem;

namespace Sas.Body.Service.Test
{
    public class BodySystemBarycenterTest
    {
        [Fact]
        public void BodySystemReturnsBarycentrum()
        {
            // Arrange
            BodySystem.Service.Models.Domain.Bodies.Body body1 = new("B1", 10, new Vector(50), new Vector(0, 1));
            BodySystem.Service.Models.Domain.Bodies.Body body2 = new("B2", 10, new Vector(0), new Vector(0, -1));

            List<BodySystem.Service.Models.Domain.Bodies.Body> bodies = [body1, body2];

            // Act
            BodySystemsModel bodySystem = new(null!, bodies); // TODO: Mock mediatR
            bodySystem.UpdateBodySystem();

            // Assert
            BodySystem.Service.Models.Domain.Bodies.Body? barycentrum = bodySystem.Barycentrum;
            barycentrum.Should().NotBeNull();
            barycentrum!.Name.Should().Be("Barycentrum");
            barycentrum.Mass.Should().Be(20);
            barycentrum.Radius.Should().Be(0);
            barycentrum.Position.X.Should().Be(0);
            barycentrum.Position.Y.Should().Be(0);
            barycentrum.Position.Z.Should().Be(0);
            barycentrum.Velocity.Should().Be(Vector.Zero);
        }

        [Fact]
        public void BodySystemReturnsBarycentrumAsNullWhenNoBodies()
        {
            BodySystemsModel bodySystem = new(null!, []);  // TODO: Mock mediatR
            bodySystem.Barycentrum.Should().BeNull();
        }

        [Fact]
        public void BodySystemReturnsBarycentrumAsBodyWhenOneBody()
        {
            BodySystem.Service.Models.Domain.Bodies.Body body1 = new("B1", 10, Vector.Ones, Vector.Ones);
            BodySystemsModel bodySystem = new(null!, [body1]); // TODO: Mock mediatR
            bodySystem.UpdateBodySystem();
            BodySystem.Service.Models.Domain.Bodies.Body? barycentrum = bodySystem.Barycentrum;
            barycentrum.Should().NotBeNull();
            barycentrum!.Name.Should().Be("Barycentrum");
            barycentrum.Mass.Should().Be(body1.Mass);
            barycentrum.Position.Should().Be(body1.Position);
            barycentrum.Velocity.Should().Be(body1.Velocity);
        }

        [Fact]
        public void CalibrateBodyForBarycentrum()
        {
            BodySystem.Service.Models.Domain.Bodies.Body body1 = new("B1", 10, Vector.Ones, Vector.Ones);
            BodySystemsModel bodySystem = new(null!, [body1]); // TODO: Mock mediatR
            bodySystem.UpdateBodySystem();
            bodySystem.CalibrateBarycenterToZero();
            BodySystem.Service.Models.Domain.Bodies.Body? barycenter = bodySystem.Barycentrum;
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
            BodySystem.Service.Models.Domain.Bodies.Body body1 = new("B1", 100, new Vector(0), new Vector(0, 1));
            BodySystem.Service.Models.Domain.Bodies.Body body2 = new("B2", 1, new Vector(50), new Vector(0, 2));
            BodySystemsModel bodySystem = new(null!, [body1, body2]); // TODO: Mock mediatR
            bodySystem.UpdateBodySystem();

            // Act
            bodySystem.CalibrateBarycenterToZero();

            // Assert
            BodySystem.Service.Models.Domain.Bodies.Body? barycenter = bodySystem.Barycentrum;
            body1.Velocity.Should().Be(Vector.Zero);
            body2.Velocity.Should().Be(new Vector(0, 1));
            barycenter!.Velocity.Should().Be(Vector.Zero);
        }
    }
}
