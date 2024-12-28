using Sas.Body.Service.Models.Domain;
using Sas.Body.Service.Models.Domain.BodyExtensions;
using Sas.Mathematica.Service.Vectors;
using Xunit;

namespace Sas.Body.Service.Test
{
    public class BodyExtensionTest
    {
        [Fact]
        public void BodyExtensionThrowsExceptionWhenArgumentIsNull()
        {
            // Arrange
            Vector position = Vector.Zero;
            Vector velocity = Vector.Zero;
            BodyDomain body = new BodyDomain("Planet", 10, position, velocity);

            // Act & Assert
            _ = Assert.Throws<ArgumentNullException>(() => body.GetPositionRelatedTo(null));
            _ = Assert.Throws<ArgumentNullException>(() => body.GetVelocityRelatedTo(null));
            _ = Assert.Throws<ArgumentNullException>(() => body.GetSphereOfInfluenceRelatedTo(null));
        }

        [Fact]
        public void ExtensionReturnsRelatedPosition()
        {
            // Arrange 
            Vector position = new Vector(10, 10, 10);
            BodyDomain sun = new BodyDomain("Sun", 100, Vector.Zero, Vector.Zero);
            BodyDomain earth = new BodyDomain("Earth", 1, position, Vector.Zero);

            // Act & Assert
            Vector expected = new Vector(10, 10, 10);
            Assert.Equal(expected, earth.GetPositionRelatedTo(sun));
            Assert.Equal(-1 * expected, sun.GetPositionRelatedTo(earth));
            Assert.Equal(Vector.Zero, earth.GetPositionRelatedTo(earth));
            Assert.Equal(Vector.Zero, sun.GetPositionRelatedTo(sun));
        }

        [Fact]
        public void ExtensionReturnsRelatedVelocity()
        {
            // Arrange 
            Vector velocity = new Vector(10, 10, 10);
            BodyDomain sun = new BodyDomain("Sun", 100, Vector.Zero, Vector.Zero);
            BodyDomain earth = new BodyDomain("Earth", 1, Vector.Zero, velocity);

            // Act & Assert
            Vector expected = new Vector(10, 10, 10);
            Assert.Equal(expected, earth.GetVelocityRelatedTo(sun));
            Assert.Equal(-1 * expected, sun.GetVelocityRelatedTo(earth));
            Assert.Equal(Vector.Zero, earth.GetVelocityRelatedTo(earth));
            Assert.Equal(Vector.Zero, sun.GetVelocityRelatedTo(sun));
        }
    }
}
