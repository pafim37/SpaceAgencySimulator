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
using Sas.Mathematica.Service.Vectors;
using Xunit;

namespace Sas.BodySystem.Tests
{
    public class BodySystemControllerTest
    {
        private readonly Mock<IBodyRepository> _bodyRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<BodySystemController>> _loggerMock = new();


        private static BodyDTO _bodyDto1 = new BodyDTO()
        {
            Name = "BodyDTO1",
            Mass = 1,
            Position = new VectorDTO { X = 1, Y = 1, Z = 1 },
            Velocity = new VectorDTO { X = 1, Y = 1, Z = 1 },
            Radius = 1,
        };

        private static BodyDTO _bodyDto2 = new BodyDTO()
        {
            Name = "BodyDTO2",
            Mass = 2,
            Position = new VectorDTO { X = 2, Y = 2, Z = 2 },
            Velocity = new VectorDTO { X = 2, Y = 2, Z = 2 },
            Radius = 2,
        };

        private static Body _body1 => new Body()
        {
            Name = "Body1",
            Mass = 1,
            Position = Vector.Ones,
            Velocity = Vector.Ones,
            Radius = 1,
        };
        private static Body _body2 => new Body()
        {
            Name = "Body2",
            Mass = 2,
            Position = Vector.Zero,
            Velocity = Vector.Zero,
            Radius = 2,
        };

        private static IEnumerable<Body> _bodyTestData => new List<Body>() {
            _body1,
            _body2
        };

        private static IEnumerable<BodyDTO> _bodyDtoTestData => new List<BodyDTO>() {
            _bodyDto1,
            _bodyDto2
        };

        private static BodySystemDTO _bodySystemDTOs => new BodySystemDTO()
        {
            Bodies = _bodyDtoTestData.ToList(),
            Orbits = new List<OrbitDTO>()
        };

        private void SetupMocks()
        {
            _bodyRepositoryMock.Setup(mock => mock.GetAllAsync())
                .ReturnsAsync(new List<BodyDocument>());

            _mapperMock.Setup(mock => mock.Map<BodySystemDTO>(It.IsAny<Sas.Domain.Models.Bodies.BodySystem>()))
                .Returns(_bodySystemDTOs);
        }

        private void SetupMocksBodyDocumentToBody()
        {
            _mapperMock.Setup(mock => mock.Map<IEnumerable<Body>>(It.IsAny<IEnumerable<BodyDocument>>()))
                .Returns(_bodyTestData);
        }

        private void SetupMocksBodyDtoToBodyDocument()
        {
            _mapperMock.Setup(mock => mock.Map<IEnumerable<BodyDocument>>(It.IsAny<IEnumerable<BodyDTO>>()))
                .Returns(new List<BodyDocument>());
        }

        [Fact]
        public async Task GetBodySystemReturnsBodySystem()
        {
            // Arrange
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
            BodySystemDTO data = okResult.Value.Should().BeAssignableTo<BodySystemDTO>().Subject;
            data.Bodies.Should().HaveCount(2);
            data.Orbits.Should().HaveCount(0); // TODO: test it!
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
            IActionResult result = await controller.CreateBodySystem(_bodyDtoTestData).ConfigureAwait(false);

            typeof(BodySystemController).Methods()
                .ThatReturn<IActionResult>()
                .ThatAreDecoratedWith<HttpPostAttribute>()
                .Should().BeAsync();
            result.Should().NotBeNull();
            OkObjectResult okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            BodySystemDTO data = okResult.Value.Should().BeAssignableTo<BodySystemDTO>().Subject;
            data.Bodies.Should().HaveCount(2);
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
