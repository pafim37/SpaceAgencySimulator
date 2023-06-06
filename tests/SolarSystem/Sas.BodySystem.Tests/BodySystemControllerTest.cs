using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sas.BodySystem.Service.Controllers;
using Sas.BodySystem.Service.DAL;
using Sas.BodySystem.Service.Documents;
using Sas.BodySystem.Service.DTOs;
using Sas.Domain.Models.Bodies;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using Xunit;

namespace Sas.BodySystem.Tests
{
    public class BodySystemControllerTest
    {
        private readonly Mock<IBodyRepository> _bodyRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<BodySystemController>> _loggerMock = new();

        private static readonly BodySystemInputData BodySystemInputData = new()
        {
            GravitationalConstant = 1,
            Bodies = new List<BodyDTO>()
            {
                new BodyDTO() { Name = "Sun", Mass = 10000, Position = new() {X = 0, Y = 0, Z =0 }, Velocity = new() {X = 0, Y = 0, Z =0 }, Radius = 10 },
                new BodyDTO() { Name = "Earth", Mass = 10, Position = new() {X = 100, Y = 0, Z =0 }, Velocity = new() {X = 0, Y = 2, Z =0 }, Radius = 1 }
            }
        };

        private static readonly BodySystemOutputData BodySystemOutputData = new()
        {
            GravitationalConstant = 1,
            Bodies = new List<BodyDTO>()
            {
                new BodyDTO() { Name = "Sun", Mass = 10000, Position = new() {X = 0, Y = 0, Z =0 }, Velocity = new() {X = 0, Y = 0, Z =0 }, Radius = 10 },
                new BodyDTO() { Name = "Earth", Mass = 10, Position = new() {X = 100, Y = 0, Z =0 }, Velocity = new() {X = 0, Y = 2, Z =0 }, Radius = 1 }
            },
            Orbits = new List<OrbitDTO>()
        };

        private void SetupMocks()
        {
            _bodyRepositoryMock.Setup(mock => mock.GetAllAsync())
                .ReturnsAsync(new List<BodyDocument>());
            _mapperMock.Setup(mock => mock.Map<BodySystemOutputData>(It.IsAny<Sas.Domain.Models.Bodies.BodySystem>()))
                .Returns(BodySystemOutputData);
            _mapperMock.Setup(mock => mock.Map<IEnumerable<Body>>(It.IsAny<IEnumerable<BodyDocument>>()))
                .Returns(new List<Body>());
            _mapperMock.Setup(mock => mock.Map<IEnumerable<BodyDocument>>(It.IsAny<IEnumerable<BodyDTO>>()))
                .Returns(new List<BodyDocument>());
        }

        [Fact]
        public async Task GetBodiesTest()
        {
            // Arrange
            SetupMocks();
            BodySystemController controller = new BodySystemController(_bodyRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
            // Act
            IActionResult result = await controller.GetBodies().ConfigureAwait(false);

            // Assert
            typeof(BodySystemController).Methods()
                .ThatReturn<IActionResult>()
                .ThatAreDecoratedWith<HttpGetAttribute>()
                .Should().BeAsync();
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            IEnumerable<BodyDTO> data = okResult.Value.Should().BeAssignableTo<IEnumerable<BodyDTO>>().Subject;
            _mapperMock.Verify(mock => mock.Map<IEnumerable<BodyDTO>>(It.IsAny<IEnumerable<BodyDocument>>()), Times.Once());
            _bodyRepositoryMock.Verify(mock => mock.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetBodySystemTest()
        {
            // Arrange
            SetupMocks();
            BodySystemController controller = new BodySystemController(_bodyRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            // Act
            IActionResult result = await controller.GetBodySystem(Constants.G).ConfigureAwait(false);

            // Assert
            typeof(BodySystemController).Methods()
                .ThatReturn<IActionResult>()
                .ThatAreDecoratedWith<HttpGetAttribute>()
                .Should().BeAsync();
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            BodySystemOutputData data = okResult.Value.Should().BeAssignableTo<BodySystemOutputData>().Subject;
            _bodyRepositoryMock.Verify(mock => mock.GetAllAsync(), Times.Once());
            _mapperMock.Verify(mock => mock.Map<IEnumerable<Body>>(It.IsAny<IEnumerable<BodyDocument>>()), Times.Once());
            _mapperMock.Verify(mock => mock.Map<BodySystemOutputData>(It.IsAny<Sas.Domain.Models.Bodies.BodySystem>()), Times.Once());
        }

        [Fact]
        public async Task SaveTest()
        {
            // Arrange
            SetupMocks();
            BodySystemController controller = new BodySystemController(_bodyRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
            // Act
            IActionResult result = await controller.Save(new List<BodyDTO>()).ConfigureAwait(false);

            // Assert
            typeof(BodySystemController).Methods()
                .ThatReturn<IActionResult>()
                .ThatAreDecoratedWith<HttpPostAttribute>()
                .Should().BeAsync();
            result.Should().NotBeNull();
            OkResult okResult = result.Should().BeOfType<OkResult>().Subject;
            _mapperMock.Verify(mock => mock.Map<IEnumerable<BodyDocument>>(It.IsAny<IEnumerable<BodyDTO>>()), Times.Once());
            _bodyRepositoryMock.Verify(mock => mock.CreateOrReplaceAsync(It.IsAny<IEnumerable<BodyDocument>>()), Times.Once());
        }

        [Fact]
        public async Task SynchronizeTest()
        {
            // Arrange
            SetupMocks();
            BodySystemController controller = new BodySystemController(_bodyRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
            // Act
            IActionResult result = await controller.Synchronize(BodySystemInputData).ConfigureAwait(false);

            // Assert
            typeof(BodySystemController).Methods()
                .ThatReturn<IActionResult>()
                .ThatAreDecoratedWith<HttpPostAttribute>()
                .Should().BeAsync();
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            _mapperMock.Verify(mock => mock.Map<IEnumerable<BodyDocument>>(It.IsAny<IEnumerable<BodyDTO>>()), Times.Once());
            _bodyRepositoryMock.Verify(mock => mock.GetAllAsync(), Times.Once());
            _bodyRepositoryMock.Verify(mock => mock.RemoveManyAsync(It.IsAny<IEnumerable<string>>()), Times.Once());
            _mapperMock.Verify(mock => mock.Map<IEnumerable<BodyDocument>>(It.IsAny<IEnumerable<BodyDTO>>()), Times.Once());
            _bodyRepositoryMock.Verify(mock => mock.CreateOrReplaceAsync(It.IsAny<IEnumerable<BodyDocument>>()), Times.Once());
        }

        [Fact]
        public async Task CreateBodySystemTest()
        {
            // Arrange
            SetupMocks();
            BodySystemController controller = new BodySystemController(_bodyRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
            // Act
            IActionResult result = await controller.Create(new BodySystemInputData()).ConfigureAwait(false);

            // Assert
            typeof(BodySystemController).Methods()
                .ThatReturn<IActionResult>()
                .ThatAreDecoratedWith<HttpPostAttribute>()
                .Should().BeAsync();
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            BodySystemOutputData data = okResult.Value.Should().BeAssignableTo<BodySystemOutputData>().Subject;
            _mapperMock.Verify(mock => mock.Map<IEnumerable<Body>>(It.IsAny<IEnumerable<BodyDTO>>()), Times.Once());
        }

        [Fact]
        public async Task DeleteFromBodySystemTest()
        {
            // Arrange
            string bodyName = "bodyName";
            BodySystemController controller = new BodySystemController(_bodyRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
            // Act
            IActionResult result = await controller.DeleteFromBodySystem(bodyName).ConfigureAwait(false);

            typeof(BodySystemController).Methods()
                .ThatReturn<IActionResult>()
                .ThatAreDecoratedWith<HttpDeleteAttribute>()
                .Should().BeAsync();
            result.Should().NotBeNull();
            NoContentResult noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
            _bodyRepositoryMock.Verify(mock => mock.RemoveAsync(It.IsAny<string>()), Times.Once());
        }
    }
}
