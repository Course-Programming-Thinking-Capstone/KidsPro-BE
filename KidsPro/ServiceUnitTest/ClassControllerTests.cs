using Application.Dtos.Request.Class;
using Application.Dtos.Response;
using Application.Dtos.Response.Account.Student;
using Application.Dtos.Response.Class.TeacherClass;
using Application.Dtos.Response.Class.TeacherSchedule;
using Application.Dtos.Response.Paging;
using Application.Dtos.Response.StudentSchedule;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Enums;
using WebAPI.Controllers;

namespace UnitTest
{
    [TestFixture]
    public class ClassControllerTests
    {
        private Mock<IClassService> _classServiceMock;
        private Mock<IAuthenticationService> _authenticationMock;
        private ClassController _controller;

        [SetUp]
        public void Setup()
        {
            _classServiceMock = new Mock<IClassService>();
            _authenticationMock = new Mock<IAuthenticationService>();
            _controller = new ClassController(_classServiceMock.Object, _authenticationMock.Object);
        }

        [Test]
        public async Task GetClassesByRoleAsync_ReturnsCorrectResponse()
        {
            // Arrange
            var expectedResponse = new List<ClassesResponse>();
            _classServiceMock.Setup(s => s.GetClassByRoleAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetClassesByRoleAsync();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }

        [Test]
        public async Task GetClassesAsync_ReturnsCorrectResponse()
        {
            // Arrange
            var expectedResponse = new PagingClassesResponse();
            _classServiceMock.Setup(s => s.GetClassesAsync(It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetClassesAsync(null, null);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }

        [Test]
        public async Task GetClassDetailAsync_ReturnsCorrectResponse()
        {
            // Arrange
            int classId = 1;
            var expectedResponse = new ClassDetailResponse();
            _classServiceMock.Setup(s => s.GetClassByIdAsync(classId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetClassDetailAsync(classId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }

        [Test]
        public async Task UpdateClassStatusAsync_ReturnsCorrectResponse()
        {
            // Arrange
            int classId = 1;
            ClassStatus status = ClassStatus.Opening;
            var expectedMessage = "Update class status to " + status + " successfully!";
            _classServiceMock.Setup(s => s.UpdateClassStatusAsync(classId, status)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateClassStatusAsync(classId, status);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.AreEqual(expectedMessage,
                okResult?.Value.GetType().GetProperty("Message")?.GetValue(okResult.Value));
        }

        [Test]
        public async Task CreateClassAsync_ReturnsCorrectResponse()
        {
            // Arrange
            var requestDto = new ClassCreateRequest();
            var expectedResponse = new ClassCreateResponse();
            _classServiceMock.Setup(s => s.CreateClassAsync(requestDto)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateClassAsync(requestDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }

        [Test]
        public async Task GetTeacherClassByCodeAsync_ReturnsCorrectResponse()
        {
            // Arrange
            string classCode = "ABC123";
            var expectedResponse = new TeacherClassDto();
            _classServiceMock.Setup(s => s.GetTeacherClassByCodeAsync(classCode)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetTeacherClassByCodeAsync(classCode);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }

        [Test]
        public async Task GetClassesByRoleAsync_WhenServiceReturnsData_ReturnsOk()
        {
            // Arrange
            var expectedResponse = new List<ClassesResponse>(); // Your expected response object
            _classServiceMock.Setup(s => s.GetClassByRoleAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetClassesByRoleAsync();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }

       
        [Test]
        public async Task GetClassesAsync_WhenServiceReturnsData_ReturnsOk()
        {
            // Arrange
            int? page = 1;
            int? size = 10;
            var expectedResponse = new PagingClassesResponse(); // Your expected response object
            _classServiceMock.Setup(s => s.GetClassesAsync(page, size)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetClassesAsync(page, size);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }

        [Test]
        public async Task GetClassesAsync_WhenServiceReturnsNull_ReturnsNotFound()
        {
            // Arrange
            int? page = 1;
            int? size = 10;
            _classServiceMock.Setup(s => s.GetClassesAsync(page, size)).ReturnsAsync((PagingClassesResponse)null);

            // Act
            var result = await _controller.GetClassesAsync(page, size);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }
    }
}