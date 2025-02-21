using FluentAssertions;
using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Body.Service.Models.Domain.BodySystems;
using Sas.Body.Service.Models.Domain.Orbits.Primitives;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Test
{
    public class BodySystemOrbitTest
    {
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
            bodySystem.UpdateBodySystem();
            bodySystem.CalibrateBarycenterToZero();

            // Assert
            Assert.Equal(OrbitType.Circular, bodySystem.Orbits.First()!.OrbitDescription.OrbitType);
        }

        public static IEnumerable<object[]> OrbitData()
        {
            // Basic plane XY
            yield return new object[] { new Vector(10, 0, 0), new Vector(0, 4, 0), 0, 0, 0 };        // Horizontal - counter clockwise
            yield return new object[] { new Vector(10, 0, 0), new Vector(0, -4, 0), 0, 0, Math.PI }; // Horizontal - clockwise
            yield return new object[] { new Vector(7.07, 7.07, 0), new Vector(-2.82843, 2.82843, 0), Math.PI / 4, 0, 0 }; // H - V - CCW
            yield return new object[] { new Vector(0, 10, 0), new Vector(-4, 0, 0), Math.PI / 2, 0, 0 };      // Vertical - counter clockwise
            yield return new object[] { new Vector(0, 10, 0), new Vector(4, 0, 0), Math.PI / 2, 0, Math.PI }; // Vertical - clockwise
            yield return new object[] { new Vector(0, 0, 10), new Vector(4, 0, 0), 0, Math.PI / 2, Math.PI / 2 }; // V

            // Plane XZ
            yield return new object[] { new Vector(0, 0, 10), new Vector(4, 0, 0), 0, Math.PI / 2, Math.PI / 2 }; // V
            yield return new object[] { new Vector(0, 0, 10), new Vector(-4, 0, 0), 0, Math.PI / 2, -Math.PI / 2 }; // V

            // Plane YZ
            yield return new object[] { new Vector(0, 0, 10), new Vector(0, 4, 0), 0, Math.PI / 2, 0 }; // V
            yield return new object[] { new Vector(0, 0, 10), new Vector(0, -4, 0), 0, Math.PI / 2, Math.PI }; // V

            // Plane YZ
            yield return new object[] { new Vector(35.35534, 0, 35.35534), new Vector(0, 1, 0), 0, Math.PI / 4, 0 };
        }

        [Theory]
        [MemberData(nameof(OrbitData))]
        public void BodySystemCalculateAnglesOfTheOrbitsCorrectly(Vector position, Vector velocity, double rotation, double theta, double eta)
        {
            // Arange
            BodyDomain sun = new("Sun", 100, Vector.Zero, Vector.Zero);
            BodyDomain testBody = new("test", 1, position, velocity);
            List<BodyDomain> bodies = [sun, testBody];

            // Act
            BodySystem bodySystem = new(bodies, 1);
            bodySystem.FullUpdate();

            // Assert
            List<Models.Domain.Orbits.PositionedOrbit> orbits = bodySystem.Orbits;
            orbits.Should().HaveCount(1);
            orbits.First().Phi.Should().BeApproximately(rotation, 0.0001);
            orbits.First().Theta.Should().BeApproximately(theta, 0.0001);
            orbits.First().Eta.Should().BeApproximately(eta, 0.0001);
        }
    }
}
