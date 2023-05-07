using Sas.Domain.Bodies;
using Sas.Domain.Bodies.BodyExtensions;
using Sas.Domain.Models.Bodies;
using Sas.Mathematica.Service.Vectors;
using Xunit;

namespace Sas.Domain.Tests
{
    public class BodyExtensionTest
    {
        [Fact]
        public void BodyExtensionThrowsExceptionWhenArgumentIsNull()
        {
            // Arrange
            Vector position = Vector.Zero;
            Vector velocity = Vector.Zero;
            Body body = new Body("Planet", 10, position, velocity);

            // Act & Assert
            _ = Assert.Throws<ArgumentNullException>(() => body.GetPositionRelatedTo(null));
            _ = Assert.Throws<ArgumentNullException>(() => body.GetVelocityRelatedTo(null));
            _ = Assert.Throws<ArgumentNullException>(() => body.GetSphereOfInfluenceRelatedTo(null));
        }

        [Fact]
        public void ExtensionReturnsRelatedPosition()
        {
            // Arrange 
            var position = new Vector(10, 10, 10);
            Body sun = new Body("Sun", 100, Vector.Zero, Vector.Zero);
            Body earth = new Body("Earth", 1, position, Vector.Zero);
            
            // Act & Assert
            var expected = new Vector(10, 10, 10);
            Assert.Equal(expected, earth.GetPositionRelatedTo(sun));
            Assert.Equal(-1*expected, sun.GetPositionRelatedTo(earth));
            Assert.Equal(Vector.Zero, earth.GetPositionRelatedTo(earth));
            Assert.Equal(Vector.Zero, sun.GetPositionRelatedTo(sun));
        } 
        
        [Fact]
        public void ExtensionReturnsRelatedVelocity()
        {
            // Arrange 
            var velocity = new Vector(10, 10, 10);
            Body sun = new Body("Sun", 100, Vector.Zero, Vector.Zero);
            Body earth = new Body("Earth", 1, Vector.Zero, velocity);

            // Act & Assert
            var expected = new Vector(10, 10, 10);
            Assert.Equal(expected, earth.GetVelocityRelatedTo(sun));
            Assert.Equal(-1*expected, sun.GetVelocityRelatedTo(earth));
            Assert.Equal(Vector.Zero, earth.GetVelocityRelatedTo(earth));
            Assert.Equal(Vector.Zero, sun.GetVelocityRelatedTo(sun));
        }
    }
}
