using FluentAssertions;
using Sas.Body.Service.Models.Domain;
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
            Assert.Equal(OrbitType.Circular, bodySystem.Orbits.First()!.OrbitType);
        }
    }
}
