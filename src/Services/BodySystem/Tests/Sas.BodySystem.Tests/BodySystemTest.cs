using Moq;
using Sas.BodySystem.Process.Models.Domain.Bodies;
using Sas.BodySystem.Process.Models.Domain.Orbits;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using BodySystemModel = global::Sas.BodySystem.Process.BodySystems.BodySystem;

namespace Sas.BodySystem.Tests
{
    public class BodySystemTest
    {
        [Fact]
        public void BodySystem_ShouldReturnBodies()
        {
            BodyDomain body1 = new()
            {
                Name = "B1",
                Mass = 10,
                Position = Vector.Ones,
                Velocity = Vector.Ones
            };
            BodySystemModel bodySystem = new([body1], CreateOrbitFactory());
            Assert.Single(bodySystem.Bodies);
        }

        [Fact]
        public void BodySystem_ShouldReturnBodySystemWithDefaultGravitationalConstant()
        {
            BodyDomain body1 = new()
            {
                Name = "B1",
                Mass = 10,
                Position = Vector.Ones,
                Velocity = Vector.Ones
            };
            BodySystemModel bodySystem = new([body1], CreateOrbitFactory());
            Assert.Equal(Constants.G, bodySystem.GravitationalConstant);
        }

        [Fact]
        public void BodySystem_ShouldReturnBodySystemWithCustomGravitationalConstant()
        {
            BodyDomain body1 = new()
            {
                Name = "B1",
                Mass = 10,
                Position = Vector.Ones,
                Velocity = Vector.Ones
            };
            BodySystemModel bodySystem = new([body1], CreateOrbitFactory(), 1);
            Assert.Equal(1, bodySystem.GravitationalConstant);
        }

        [Fact]
        public void BodySystem_UpdateBodySystem_ShouldUpdateBodySystem()
        {
            // Arrange
            BodyDomain star = new()
            {
                Name = "Sun",
                Mass = 100,
                Position = Vector.Zero,
                Velocity = Vector.Zero
            };
            BodyDomain planet = new()
            {
                Name = "Earth",
                Mass = 1,
                Position = Vector.Ox,
                Velocity = Vector.Oy
            };
            BodySystemModel bodySystem = new([star, planet], CreateOrbitFactory(), gravitationalConstant: 1);

            // Act
            bodySystem.UpdateBodySystem(scale: 0, atTime: 0, CancellationToken.None);

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
            BodyDomain body1 = new()
            {
                Name = "B1",
                Mass = 10,
                Position = new Vector(50),
                Velocity = new Vector(0, 1)
            };
            BodyDomain body2 = new()
            {
                Name = "B2",
                Mass = 10,
                Position = new Vector(0),
                Velocity = new Vector(0, -1)
            };

            List<BodyDomain> bodies = [body1, body2];

            // Act
            BodySystemModel bodySystem = new(bodies, CreateOrbitFactory());
            bodySystem.UpdateBodySystem(scale: 0, atTime: 0, CancellationToken.None);

            // Assert
            BodyDomain? barycentrum = bodySystem.Barycentrum;
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
            BodyDomain body1 = new()
            {
                Name = "B1",
                Mass = 10,
                Position = Vector.Ones,
                Velocity = Vector.Ones
            };
            BodySystemModel bodySystem = new([body1], CreateOrbitFactory());
            bodySystem.UpdateBodySystem(scale: 0, atTime: 0, CancellationToken.None);
            bodySystem.CalibrateBarycenterToZero();
            BodyDomain? barycenter = bodySystem.Barycentrum;
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
            BodyDomain body1 = new()
            {
                Name = "B1",
                Mass = 100,
                Position = new Vector(0),
                Velocity = new Vector(0, 1)
            };
            BodyDomain body2 = new()
            {
                Name = "B2",
                Mass = 1,
                Position = new Vector(50),
                Velocity = new Vector(0, 2)
            };
            BodySystemModel bodySystem = new([body1, body2], CreateOrbitFactory());
            bodySystem.UpdateBodySystem(scale: 0, atTime: 0, CancellationToken.None);

            // Act
            bodySystem.CalibrateBarycenterToZero();

            // Assert
            BodyDomain? barycenter = bodySystem.Barycentrum;
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
            BodyDomain body1 = new()
            {
                Name = "B1",
                Mass = 10,
                Position = Vector.Ones,
                Velocity = Vector.Ones
            };
            BodySystemModel bodySystem = new([body1], CreateOrbitFactory());
            bodySystem.UpdateBodySystem(scale: 0, atTime: 0, CancellationToken.None);
            BodyDomain? barycentrum = bodySystem.Barycentrum;
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
            BodyDomain star = new()
            {
                Name = "Sun",
                Mass = 1000,
                Position = Vector.Zero,
                Velocity = Vector.Zero
            };
            BodyDomain planet = new()
            {
                Name = "Earth",
                Mass = 10,
                Position = 100 * Vector.Ox,
                Velocity = Vector.Oy
            };
            BodyDomain sattelite = new()
            {
                Name = "Probe",
                Mass = 0.01,
                Position = 100 * Vector.Ox + Vector.Oy,
                Velocity = -Vector.Ox
            };
            BodySystemModel bodySystem = new([planet, sattelite, star], CreateOrbitFactory());

            // Act
            bodySystem.UpdateBodySystem(scale: 0, atTime: 0, CancellationToken.None);

            // Assert
            Assert.Equal(null!, star.ParentName);
            Assert.Equal("Sun", planet.ParentName);
            Assert.Equal("Earth", sattelite.ParentName);
            Assert.NotNull(planet.SphereOfInfluenceRadius);
            Assert.NotNull(sattelite.SphereOfInfluenceRadius);
            return Task.CompletedTask;
        }

        private static IOrbitFactory CreateOrbitFactory()
        {
            Mock<IOrbit> orbit = new();
            orbit.SetupGet(o => o.Name).Returns("Test orbit");
            orbit.SetupGet(o => o.Points).Returns([]);

            Mock<IOrbitFactory> factory = new();
            factory.Setup(f => f.Create(It.IsAny<string>(), It.IsAny<Vector>(), It.IsAny<Vector>(),
                It.IsAny<Vector>(), It.IsAny<double>())).Returns(orbit.Object);
            return factory.Object;
        }
    }
}
