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
        public void BodySystem_ShouldReturnBodySystemWithCustomGravitationalConstant()
        {
            Process.Models.Domain.Bodies.Body body1 = new("B1", 10, Vector.Ones, Vector.Ones);
            BodySystemModel bodySystem = new([body1], CreateOrbitFactory(), 1);
            Assert.Equal(1, bodySystem.GravitationalConstant);
        }

        [Fact]
        public void BodySystem_UpdateBodySystem_ShouldUpdateBodySystem()
        {
            // Arrange
            Process.Models.Domain.Bodies.Body star = new("Sun", 100, Vector.Zero, Vector.Zero);
            Process.Models.Domain.Bodies.Body planet = new("Earth", 1, Vector.Ox, Vector.Oy);
            BodySystemModel bodySystem = new([star, planet], CreateOrbitFactory(), gravitationalConstant: 1);

            // Act
            bodySystem.UpdateBodySystem(CancellationToken.None);

            // Assert
            // Create barycenter
            Assert.NotNull(bodySystem.Barycentrum);
            // Perform hierarchy
            Assert.Equal(null!, star.ParentName);
            Assert.Equal("Sun", planet.ParentName);
            // Calibrate barycenter to zero
            Assert.Equal(Vector.Zero, bodySystem.Barycentrum.Position);
            // Finds orbits
            Assert.Single(bodySystem.Orbits);
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

        [Fact]
        public Task BodySystem_EstablishHierarchy()
        {
            // Arrange
            BodySystem.Process.Models.Domain.Bodies.Body star = new("Sun", 1000, Vector.Zero, Vector.Zero);
            BodySystem.Process.Models.Domain.Bodies.Body planet = new("Earth", 10, 100 * Vector.Ox, Vector.Oy);
            BodySystem.Process.Models.Domain.Bodies.Body sattelite = new("Probe", 0.01, 100 * Vector.Ox + Vector.Oy, -Vector.Ox);
            BodySystemModel bodySystem = new([planet, sattelite, star], CreateOrbitFactory());

            // Act
            bodySystem.UpdateBodySystem(CancellationToken.None);

            // Assert
            Assert.Equal(null!, star.ParentName);
            Assert.Equal("Sun", planet.ParentName);
            Assert.Equal("Earth", sattelite.ParentName);
            Assert.NotNull(planet.SphereOfInfluenceRadius);
            Assert.NotNull(sattelite.SphereOfInfluenceRadius);
            return Task.CompletedTask;
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
