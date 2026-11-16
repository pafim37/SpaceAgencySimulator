using Sas.Docker.Tests;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Models;
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
        public async Task GetPositionedOrbit_ReturnsOrbit()
        {
            // Arrange
            TwoBodyState twoBodyState = new()
            {
                Name = "Earth",
                Position = new(Constants.EarthPeriapsis, 0, 0),
                Velocity = new(0, Constants.EarthMaxVelocity, 0),
                GravitationalParameter = EarthGravitationalParameter,
                GravitationalSourcePosition = Vector.Zero
            };
            using HttpRequestMessage request = new(HttpMethod.Get, "/orbit")
            {
                Content = JsonContent.Create(twoBodyState)
            };

            // Act
            using HttpResponseMessage response = await fixture.Client.SendAsync(request);

            // Assert TODO: improve this assertion
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            JsonElement orbit = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.Equal("Earth", orbit.GetProperty("name").GetString());
            Assert.Equal((int)OrbitType.Elliptic, orbit.GetProperty("orbitElements").GetProperty("orbitType").GetInt32());

            JsonElement.ArrayEnumerator points = orbit.GetProperty("points").EnumerateArray();
            Assert.NotEmpty(points);
            Assert.All(points, point =>
            {
                Assert.True(double.IsFinite(point.GetProperty("x").GetDouble()));
                Assert.True(double.IsFinite(point.GetProperty("y").GetDouble()));
                Assert.Equal(0.0, point.GetProperty("z").GetDouble(), precision: 6);
            });
        }

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
