using Sas.BodySystem.Process.Models.DataTransferObject;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using System.Net;
using System.Net.Http.Json;
using Xunit;
namespace Sas.Integration.Tests
{
    [Collection("Docker")]
    public class BodySystemScaleTest(DockerFixture fixture) : BodyDatabaseTest(fixture)
    {
        [Fact]
        public async Task BodySystem_ScaleDoesntChangeTheOrbitShape()
        {
            // TODO: Chcek this test
            // Arrange
            string[] names = ["Earth"];
            using HttpResponseMessage responseDefaults = await Client.PostAsJsonAsync("/body/defaults", names);
            Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);

            // Act
            using HttpResponseMessage response = await Client.GetAsync("/body-system");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            BodySystemDto? bodySystem = await response.Content.ReadFromJsonAsync<BodySystemDto>();

            // Gravitational Constant
            Assert.NotNull(bodySystem);
            Assert.Equal(Constants.G, bodySystem.GravitationalConstant);

            // Barycentrum
            Assert.NotNull(bodySystem.Barycentrum);
            Assert.Equal("Barycentrum", bodySystem.Barycentrum.Name);
            Assert.Equal(Vector.Zero, bodySystem.Barycentrum.Position);
            Assert.Equal(Vector.Zero, bodySystem.Barycentrum.Velocity);
            Assert.Equal(0, bodySystem.Barycentrum.Radius);
            Assert.Null(bodySystem.Barycentrum.ParentName);
            Assert.Null(bodySystem.Barycentrum.SphereOfInfluenceRadius);

            // Bodies
            Assert.NotEmpty(bodySystem.Bodies);
            Assert.Contains(bodySystem.Bodies, body => body.Name == "Sun");
            Assert.Contains(bodySystem.Bodies, body => body.Name == "Earth");
            Assert.Null(bodySystem.Bodies.First(body => body.Name == "Sun").ParentName);
            Assert.Equal("Sun", bodySystem.Bodies.First(body => body.Name == "Earth").ParentName);

            // Orbits
            Assert.NotEmpty(bodySystem.Orbits);
            Assert.Single(bodySystem.Orbits);
            Assert.Collection(bodySystem.Orbits,
                orbit1 => Assert.Equal("Earth", orbit1.Name)
            );
        }
    }
}
