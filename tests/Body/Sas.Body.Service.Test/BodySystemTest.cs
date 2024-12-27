using FluentAssertions;
using Sas.Body.Service.Models.Domain;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Test
{
    public class BodySystemTest
    {
        [Fact]
        public void BodySystemReturnsBarycentrum()
        {
            BodyDomain body1 = new BodyDomain("B1", 10, new Vector(50), new Vector(0, 1));
            BodyDomain body2 = new BodyDomain("B2", 10, new Vector(0), new Vector(0, -1));
            
            List<BodyDomain> bodies = [body1, body2];
            BodySystem bodySystem = new BodySystem(bodies);

            BodyDomain barycentrum = bodySystem.Barycentrum;

            barycentrum.Should().NotBeNull();
            barycentrum.Name.Should().Be("Barycentrum");
            barycentrum.Mass.Should().Be(20);
            barycentrum.Radius.Should().Be(0);
            barycentrum.Position.X.Should().Be(25);
            barycentrum.Position.Y.Should().Be(0);
            barycentrum.Position.Z.Should().Be(0);
            barycentrum.Velocity.Should().Be(Vector.Zero);
        }
    }
}
