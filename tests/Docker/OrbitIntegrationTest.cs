using Sas.Mathematica.Service;
using Sas.Orbit.Service.Models.DataTransferObject;
using Sas.Orbit.Service.Orbits.Primitives;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Sas.Integration.Tests
{
    [Collection("Docker")]
    public class OrbitIntegrationTest(DockerFixture fixture)
    {
        private static readonly double EarthGravitationalParameter =
            Constants.G * (Constants.SolarMass + Constants.EarthMass);

        [Fact]
        public async Task GetOrbitElements_ReturnsOrbitElements()
        {
            // Arrange
            double radius = Constants.EarthPeriapsis;
            double speed = Constants.EarthMaxVelocity;
            double expectedEccentricity = radius * speed * speed / EarthGravitationalParameter - 1;
            double expectedSemiMajorAxis = radius / (1 - expectedEccentricity);
            OrbitalStateDto orbitalState = new()
            {
                Position = new(radius, 0, 0),
                Velocity = new(0, speed, 0),
                GravitationalParameter = EarthGravitationalParameter
            };
            using HttpRequestMessage request = new(HttpMethod.Get, "/orbit/elements")
            {
                Content = JsonContent.Create(orbitalState)
            };

            // Act
            using HttpResponseMessage response = await fixture.Client.SendAsync(request);

            // Assert TODO: improve this assertion
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            JsonElement elements = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.Equal((int)OrbitType.Elliptic, elements.GetProperty("orbitType").GetInt32());
            Assert.Equal(expectedEccentricity, elements.GetProperty("eccentricity").GetDouble(), precision: 9);
            Assert.Equal(1.0, elements.GetProperty("semiMajorAxis").GetDouble() / expectedSemiMajorAxis, precision: 9);
            Assert.Equal(0.0, elements.GetProperty("inclination").GetDouble(), precision: 9);
        }
    }
}
