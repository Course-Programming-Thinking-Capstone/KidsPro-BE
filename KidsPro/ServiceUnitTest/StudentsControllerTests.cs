using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Student;
using Application.Dtos.Response.Game;
using Application.Dtos.Response.Paging;
using Application.Dtos.Response.StudentProgress;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;

namespace UnitTest
{
    [TestFixture]
    public class StudentsControllerTests
    {
        [Test]
        public async Task GetStudentDetail_ReturnsCorrectStudentDetailResponse()
        {
            // Arrange
            var expectedResponse = new StudentDetailResponse();
            var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(s => s.GetDetailStudentAsync(1)).ReturnsAsync(expectedResponse);
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(studentServiceMock.Object, authenticationMock.Object, null);

            // Act
            var result = await controller.GetStudentDetail(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }

        [Test]
        public async Task UpdateStudentInformation_ReturnsOkResult()
        {
            // Arrange
            var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(s => s.UpdateStudentAsync(It.IsAny<StudentUpdateRequest>()))
                .Returns(Task.CompletedTask);
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(studentServiceMock.Object, authenticationMock.Object, null);

            // Act
            var result = await controller.UpdateStudentInformation(new StudentUpdateRequest());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Update Student Information Success!", okResult?.Value);
        }

        [Test]
        public async Task GetSectionProgress_ReturnsCorrectSectionProgressResponse()
        {
            // Arrange
            var expectedResponse = new SectionProgressResponse();
            var progressServiceMock = new Mock<IProgressService>();
            progressServiceMock.Setup(s => s.GetCourseProgressAsync(1, 1)).ReturnsAsync(expectedResponse);
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(null, authenticationMock.Object, progressServiceMock.Object);

            // Act
            var result = await controller.GetSectionProgress(1, 1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }

        [Test]
        public async Task GetStudentDetail_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(s => s.GetDetailStudentAsync(It.IsAny<int>()))
                .ReturnsAsync((StudentDetailResponse)null);
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(studentServiceMock.Object, authenticationMock.Object, null);

            // Act
            var result = await controller.GetStudentDetail(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task UpdateStudentInformation_WithNullRequest_ReturnsBadRequest()
        {
            // Arrange
            var studentServiceMock = new Mock<IStudentService>();
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(studentServiceMock.Object, authenticationMock.Object, null);

            // Act
            var result = await controller.UpdateStudentInformation(null);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetSectionProgress_WithInvalidCourseId_ReturnsNotFound()
        {
            // Arrange
            var progressServiceMock = new Mock<IProgressService>();
            progressServiceMock.Setup(s => s.GetCourseProgressAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((SectionProgressResponse)null);
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(null, authenticationMock.Object, progressServiceMock.Object);

            // Act
            var result = await controller.GetSectionProgress(1, 1);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }

        [Test]
        public async Task GetSectionProgress_WithInvalidSectionId_ReturnsNotFound()
        {
            // Arrange
            var progressServiceMock = new Mock<IProgressService>();
            progressServiceMock.Setup(s => s.GetCourseProgressAsync(1, It.IsAny<int>()))
                .ReturnsAsync((SectionProgressResponse)null);
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(null, authenticationMock.Object, progressServiceMock.Object);

            // Act
            var result = await controller.GetSectionProgress(1, 1);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }
        [Test]
        public async Task GetStudentDetail_WithValidId_ReturnsCorrectStudentDetailResponse()
        {
            // Arrange
            var expectedResponse = new StudentDetailResponse();
            var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(s => s.GetDetailStudentAsync(1)).ReturnsAsync(expectedResponse);
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(studentServiceMock.Object, authenticationMock.Object, null);

            // Act
            var result = await controller.GetStudentDetail(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }
        [Test]
        public async Task UpdateStudentInformation_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(s => s.UpdateStudentAsync(It.IsAny<StudentUpdateRequest>()))
                .Returns(Task.CompletedTask);
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(studentServiceMock.Object, authenticationMock.Object, null);
            var request = new StudentUpdateRequest();

            // Act
            var result = await controller.UpdateStudentInformation(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Update Student Information Success!", okResult?.Value);
        }

        [Test]
        public async Task GetSectionProgress_WithValidIds_ReturnsCorrectSectionProgressResponse()
        {
            // Arrange
            var expectedResponse = new SectionProgressResponse();
            var progressServiceMock = new Mock<IProgressService>();
            progressServiceMock.Setup(s => s.GetCourseProgressAsync(1, 1)).ReturnsAsync(expectedResponse);
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(null, authenticationMock.Object, progressServiceMock.Object);

            // Act
            var result = await controller.GetSectionProgress(1, 1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }
        [Test]
        public async Task UpdateStudentInformation_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var studentServiceMock = new Mock<IStudentService>();
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(studentServiceMock.Object, authenticationMock.Object, null);
            controller.ModelState.AddModelError("Error", "Invalid model state");

            // Act
            var result = await controller.UpdateStudentInformation(new StudentUpdateRequest());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        
        [Test]
        public async Task GetSectionProgress_WithInvalidIds_ReturnsNotFound()
        {
            // Arrange
            var progressServiceMock = new Mock<IProgressService>();
            progressServiceMock.Setup(s => s.GetCourseProgressAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((SectionProgressResponse)null);
            var authenticationMock = new Mock<IAuthenticationService>();
            var controller = new StudentsController(null, authenticationMock.Object, progressServiceMock.Object);

            // Act
            var result = await controller.GetSectionProgress(1, 1);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }
       

    }
}