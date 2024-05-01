using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Student;
using Application.Dtos.Response.StudentProgress;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            studentServiceMock.Setup(s => s.UpdateStudentAsync(It.IsAny<StudentUpdateRequest>())).Returns(Task.CompletedTask);
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
    }
}
