using Sas.Body.Core.DataTransferObject;
using Sas.BodySystem.Process.Models.DataTransferObject;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Sas.Integration.Tests
{
    [Collection("Docker")]
    public class BodySystemIntegrationTest(DockerFixture fixture)
    {
        [Fact]
        public async Task BodySystem_ReturnsBodySystem()
        {
            // Arrange
            string[] names = ["Earth", "Moon"];
            using HttpResponseMessage responseDefaults = await fixture.Client.PostAsJsonAsync("/body/defaults", names);
            Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);

            using HttpResponseMessage enableSun = await fixture.Client.PostAsJsonAsync("/body/Sun", true);
            Assert.Equal(HttpStatusCode.OK, enableSun.StatusCode);

            // Act
            using HttpResponseMessage response = await fixture.Client.GetAsync("/body-system");

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
            Assert.Contains(bodySystem.Bodies, body => body.Name == "Moon");
            Assert.Null(bodySystem.Bodies.First(body => body.Name == "Sun").ParentName);
            Assert.Equal("Sun", bodySystem.Bodies.First(body => body.Name == "Earth").ParentName);
            Assert.Equal("Earth", bodySystem.Bodies.First(body => body.Name == "Moon").ParentName);

            // Orbits
            Assert.NotEmpty(bodySystem.Orbits);
            Assert.Equal(2, bodySystem.Orbits.Count);
            Assert.Collection(bodySystem.Orbits,
                orbit1 => Assert.Equal("Earth", orbit1.Name),
                orbit2 => Assert.Equal("Moon", orbit2.Name)
            );

            // Delete the created bodies
            await DeleteBodies(names);
        }

        private async Task DeleteBodies(string[] names)
        {
            foreach (string name in names)
            {
                using HttpResponseMessage responseDelete = await fixture.Client.DeleteAsync($"/body/{name}");
                Assert.Equal(HttpStatusCode.OK, responseDelete.StatusCode);
            }
            using HttpResponseMessage responseGet = await fixture.Client.GetAsync("/body");
            List<BodyDto>? bodies = await responseGet.Content.ReadFromJsonAsync<List<BodyDto>>();
            Assert.NotNull(bodies);
            Assert.DoesNotContain(bodies, body => names.Contains(body!.Name));
        }

    }
}
