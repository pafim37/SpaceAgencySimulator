using Moq;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits;
using BodySystemModel = global::Sas.BodySystem.Process.BodySystems.BodySystem;

namespace Sas.BodySystem.Tests
{
    public class BodySystemTest
    {
        [Fact]
        public void BodySystem_ShouldReturnBodies()
        {
            Process.Models.Domain.Bodies.Body body1 = new("B1", 10, Vector.Ones, Vector.Ones);
            BodySystemModel bodySystem = new([body1], CreateOrbitFactory());
            Assert.Single(bodySystem.Bodies);
        }

        [Fact]
        public void BodySystem_ShouldReturnBodySystemWithDefaultGravitationalConstant()
        {
            Process.Models.Domain.Bodies.Body body1 = new("B1", 10, Vector.Ones, Vector.Ones);
            BodySystemModel bodySystem = new([body1], CreateOrbitFactory());
            Assert.Equal(Constants.G, bodySystem.GravitationalConstant);
        }

        [Fact]
        public void BodySystemReturnsBarycentrum()
        {
            // Arrange
            BodySystem.Process.Models.Domain.Bodies.Body body1 = new("B1", 10, new Vector(50), new Vector(0, 1));
            BodySystem.Process.Models.Domain.Bodies.Body body2 = new("B2", 10, new Vector(0), new Vector(0, -1));

            List<BodySystem.Process.Models.Domain.Bodies.Body> bodies = [body1, body2];

            // Act
            BodySystemModel bodySystem = new(bodies, CreateOrbitFactory());
            bodySystem.UpdateBodySystem(CancellationToken.None);

            // Assert
            BodySystem.Process.Models.Domain.Bodies.Body? barycentrum = bodySystem.Barycentrum;
            Assert.NotNull(barycentrum);
            Assert.Equal("Barycentrum", barycentrum!.Name);
            Assert.Equal(20, barycentrum.Mass);
            Assert.Equal(0, barycentrum.Radius);
            Assert.Equal(0, barycentrum.Position.X);
            Assert.Equal(0, barycentrum.Position.Y);
            Assert.Equal(0, barycentrum.Position.Z);
            Assert.Equal(Vector.Zero, barycentrum.Velocity);
        }

        [Fact]
        public void CalibrateBodyForBarycentrum()
        {
            BodySystem.Process.Models.Domain.Bodies.Body body1 = new("B1", 10, Vector.Ones, Vector.Ones);
            BodySystemModel bodySystem = new([body1], CreateOrbitFactory());
            bodySystem.UpdateBodySystem(CancellationToken.None);
            bodySystem.CalibrateBarycenterToZero();
            BodySystem.Process.Models.Domain.Bodies.Body? barycenter = bodySystem.Barycentrum;
            Assert.Equal(Vector.Zero, body1.Position);
            Assert.Equal(Vector.Zero, body1.Velocity);
            Assert.NotNull(barycenter);
            Assert.Equal(Vector.Zero, barycenter!.Position);
            Assert.Equal(Vector.Zero, barycenter.Velocity);
        }

        [Fact]
        public void CalibrateBodiesVelocityForBarycentrum()
        {
            // Arrange
            BodySystem.Process.Models.Domain.Bodies.Body body1 = new("B1", 100, new Vector(0), new Vector(0, 1));
            BodySystem.Process.Models.Domain.Bodies.Body body2 = new("B2", 1, new Vector(50), new Vector(0, 2));
            BodySystemModel bodySystem = new([body1, body2], CreateOrbitFactory());
            bodySystem.UpdateBodySystem(CancellationToken.None);

            // Act
            bodySystem.CalibrateBarycenterToZero();

            // Assert
            BodySystem.Process.Models.Domain.Bodies.Body? barycenter = bodySystem.Barycentrum;
            Assert.Equal(Vector.Zero, body1.Velocity);
            Assert.Equal(new Vector(0, 1), body2.Velocity);
            Assert.Equal(Vector.Zero, barycenter!.Velocity);
        }

        [Fact]
        public void BodySystemReturnsBarycentrumAsNullWhenNoBodies()
        {
            BodySystemModel bodySystem = new([], CreateOrbitFactory());
            Assert.Null(bodySystem.Barycentrum);
        }

        [Fact]
        public void BodySystemReturnsBarycentrumAsBodyWhenOneBody()
        {
            BodySystem.Process.Models.Domain.Bodies.Body body1 = new("B1", 10, Vector.Ones, Vector.Ones);
            BodySystemModel bodySystem = new([body1], CreateOrbitFactory());
            bodySystem.UpdateBodySystem(CancellationToken.None);
            BodySystem.Process.Models.Domain.Bodies.Body? barycentrum = bodySystem.Barycentrum;
            Assert.NotNull(barycentrum);
            Assert.Equal("Barycentrum", barycentrum!.Name);
            Assert.Equal(body1.Mass, barycentrum.Mass);
            Assert.Equal(body1.Position, barycentrum.Position);
            Assert.Equal(body1.Velocity, barycentrum.Velocity);
        }

        private static IPositionedOrbitFactory CreateOrbitFactory()
        {
            Mock<IPositionedOrbit> orbit = new();
            orbit.SetupGet(o => o.Name).Returns("Test orbit");
            orbit.SetupGet(o => o.Points).Returns([]);

            Mock<IPositionedOrbitFactory> factory = new();
            factory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<Vector>(), It.IsAny<Vector>(),
                It.IsAny<Vector>(), It.IsAny<double>())).Returns(orbit.Object);
            return factory.Object;
        }
    }
}
