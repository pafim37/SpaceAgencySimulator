using Sas.BodySystem.Process.Extensions;
using Sas.BodySystem.Process.Models.Domain.Bodies;
using Sas.Mathematica.Service.Vectors;

namespace Sas.BodySystem.Tests
{
    public class BodyExtensionTest
    {
        [Fact]
        public void ExtensionReturnsRelatedPosition()
        {
            // Arrange 
            Vector position = new(10, 10, 10);
            BodyDomain sun = new()
            {
                Name = "Sun",
                Mass = 100,
                Position = Vector.Zero,
                Velocity = Vector.Zero
            };
            BodyDomain earth = new()
            {
                Name = "Earth",
                Mass = 1,
                Position = position,
                Velocity = Vector.Zero
            };

            // Act & Assert
            Vector expected = new(10, 10, 10);
            Assert.Equal(expected, earth.GetPositionRelatedTo(sun));
            Assert.Equal(-1 * expected, sun.GetPositionRelatedTo(earth));
            Assert.Equal(Vector.Zero, earth.GetPositionRelatedTo(earth));
            Assert.Equal(Vector.Zero, sun.GetPositionRelatedTo(sun));
        }

        [Fact]
        public void ExtensionReturnsRelatedVelocity()
        {
            // Arrange 
            Vector velocity = new(10, 10, 10);
            BodyDomain sun = new()
            {
                Name = "Sun",
                Mass = 100,
                Position = Vector.Zero,
                Velocity = Vector.Zero
            };
            BodyDomain earth = new()
            {
                Name = "Earth",
                Mass = 1,
                Position = Vector.Zero,
                Velocity = velocity
            };

            // Act & Assert
            Vector expected = new(10, 10, 10);
            Assert.Equal(expected, earth.GetVelocityRelatedTo(sun));
            Assert.Equal(-1 * expected, sun.GetVelocityRelatedTo(earth));
            Assert.Equal(Vector.Zero, earth.GetVelocityRelatedTo(earth));
            Assert.Equal(Vector.Zero, sun.GetVelocityRelatedTo(sun));
        }
    }
}
