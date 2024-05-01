using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Account.Parent;
using Application.Dtos.Response.Teacher;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos.Response.Account.Student;
using Domain.Entities;
using Domain.Enums;
using WebAPI.Controllers;

namespace UnitTest
{
    [TestFixture]
    public class ParentsControllerTests
    {
        private Mock<IParentsService> _parentServiceMock;
        private Mock<IAuthenticationService> _authenticationMock;
        private Mock<ITeacherService> _teacherServiceMock;
        private ParentsController _controller;

        [SetUp]
        public void Setup()
        {
            _parentServiceMock = new Mock<IParentsService>();
            _authenticationMock = new Mock<IAuthenticationService>();
            _teacherServiceMock = new Mock<ITeacherService>();

            _controller = new ParentsController(
                _parentServiceMock.Object,
                _authenticationMock.Object,
                _teacherServiceMock.Object
            );
        }

        [Test]
        public async Task AddStudent_ReturnsOkResult()
        {
            // Arrange
            var request = new StudentAddRequest();
            var expectedResult = new StudentResponse();

            _parentServiceMock.Setup(service => service.AddStudentAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AddStudent(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedResult, okResult.Value);
        }

        [Test]
        public async Task GetEmailZalo_ReturnsOkResult()
        {
            // Arrange
            var expectedResult = new ParentOrderResponse();

            _parentServiceMock.Setup(service => service.GetParentEmail())
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetEmailZalo();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedResult, okResult.Value);
        }

        [Test]
        public async Task GetListVoucherAsync_ReturnsOkResult()
        {
            // Arrange
            var status = VoucherStatus.Valid;
            var expectedResult = new List<GameVoucher>();

            _parentServiceMock.Setup(service => service.GetListVoucherAsync(status))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetListVoucherAsync(status);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedResult, okResult.Value);
        }

        [Test]
        public async Task GetTeachersAsync_ReturnsOkResult()
        {
            // Arrange
            var expectedResult = new List<TeacherResponse>();

            _teacherServiceMock.Setup(service => service.GetTeachers())
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetTeachersAsync();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedResult, okResult.Value);
        }

        [Test]
        public async Task GetTeachersDetailAsync_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            var expectedResult = new AccountDto();

            _teacherServiceMock.Setup(service => service.GetTeacherDetail(id))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetTeachersDetailAsync(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedResult, okResult.Value);
        }
    }
}
