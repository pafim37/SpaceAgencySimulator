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


        private static readonly BodyDTO BodyDto1 = new()
        {
            Name = "BodyDTO1",
            Mass = 1,
            Position = new VectorDTO { X = 1, Y = 1, Z = 1 },
            Velocity = new VectorDTO { X = 1, Y = 1, Z = 1 },
            Radius = 1,
        };

        private static readonly BodyDTO BodyDto2 = new()
        {
            Name = "BodyDTO2",
            Mass = 2,
            Position = new VectorDTO { X = 2, Y = 2, Z = 2 },
            Velocity = new VectorDTO { X = 2, Y = 2, Z = 2 },
            Radius = 2,
        };

        private static Body Body1 => new()
        {
            Name = "Body1",
            Mass = 1,
            Position = Vector.Ones,
            Velocity = Vector.Ones,
            Radius = 1,
        };
        private static Body Body2 => new()
        {
            Name = "Body2",
            Mass = 2,
            Position = Vector.Zero,
            Velocity = Vector.Zero,
            Radius = 2,
        };

        private static IEnumerable<Body> BodyTestData => new List<Body>() {
            Body1,
            Body2
        };

        private static IEnumerable<BodyDTO> BodyDtoTestData => new List<BodyDTO>() {
            BodyDto1,
            BodyDto2
        };

        private static BodySystemInputData BodySystemInputData => new()
        {
            GravitationalConstant = Constants.G,
            Bodies = BodyDtoTestData.ToList()
        };

        private static BodySystemOutputData BodySystemDTOs => new()
        {
            GravitationalConstant = Constants.G,
            Bodies = BodyDtoTestData.ToList(),
            Orbits = new List<OrbitDTO>()
        };

        private void SetupMocks()
        {
            _bodyRepositoryMock.Setup(mock => mock.GetAllAsync())
                .ReturnsAsync(new List<BodyDocument>());

            _mapperMock.Setup(mock => mock.Map<BodySystemOutputData>(It.IsAny<Sas.Domain.Models.Bodies.BodySystem>()))
                .Returns(BodySystemDTOs);
        }

        private void SetupMocksBodyDocumentToBody()
        {
            _mapperMock.Setup(mock => mock.Map<IEnumerable<Body>>(It.IsAny<IEnumerable<BodyDocument>>()))
                .Returns(BodyTestData);
        }

        private void SetupMocksBodyDtoToBodyDocument()
        {
            _mapperMock.Setup(mock => mock.Map<IEnumerable<BodyDocument>>(It.IsAny<IEnumerable<BodyDTO>>()))
                .Returns(new List<BodyDocument>());
        }

        [Fact]
        public async Task GetBodySystemReturnsBodySystem()
        {
            SetupMocks();
            SetupMocksBodyDocumentToBody();
            BodySystemController controller = new BodySystemController(_bodyRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            // Act
            IActionResult result = await controller.GetBodySystem().ConfigureAwait(false);

            // Act
            typeof(BodySystemController).Methods()
                .ThatReturn<IActionResult>()
                .ThatAreDecoratedWith<HttpGetAttribute>()
                .Should().BeAsync();
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            BodySystemOutputData data = okResult.Value.Should().BeAssignableTo<BodySystemOutputData>().Subject;
            data.Bodies.Should().HaveCount(2);
            data.Orbits.Should().HaveCount(0);
            data.GravitationalConstant.Should().Be(Constants.G);
            _bodyRepositoryMock.Verify(mock => mock.GetAllAsync(), Times.Once());
            _mapperMock.Verify(mock => mock.Map<IEnumerable<Body>>(It.IsAny<IEnumerable<BodyDocument>>()), Times.Once());
        }

        [Fact]
        public async Task CreateBodySystemReturnsBodySystem()
        {
            // Arrange
            SetupMocks();
            SetupMocksBodyDtoToBodyDocument();
            BodySystemController controller = new BodySystemController(_bodyRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
            // Act
            IActionResult result = await controller.CreateBodySystem(BodySystemInputData).ConfigureAwait(false);

            typeof(BodySystemController).Methods()
                .ThatReturn<IActionResult>()
                .ThatAreDecoratedWith<HttpPostAttribute>()
                .Should().BeAsync();
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            BodySystemOutputData data = okResult.Value.Should().BeAssignableTo<BodySystemOutputData>().Subject;
            data.Bodies.Should().HaveCount(2);
            data.Orbits.Should().HaveCount(0);
            data.GravitationalConstant.Should().Be(Constants.G);
            _bodyRepositoryMock.Verify(mock => mock.CreateOrReplaceAsync(It.IsAny<IEnumerable<BodyDocument>>()), Times.Once());
            _mapperMock.Verify(mock => mock.Map<IEnumerable<BodyDocument>>(It.IsAny<IEnumerable<BodyDTO>>()), Times.Once());
        }

        [Fact]
        public async Task DeleteFromBodySystemReturnsNoContent()
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
