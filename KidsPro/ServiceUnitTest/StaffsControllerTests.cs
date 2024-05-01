using Application.Dtos.Request.Account.Student;
using Application.Dtos.Request.Email;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;

namespace UnitTest
{
    public class StaffsControllerTests
    {
        private StaffsController _controller;
        private Mock<IStaffService> _staffServiceMock;
        private Mock<INotificationService> _notificationServiceMock;
        private Mock<IAuthenticationService> _authenticationServiceMock;

        [SetUp]
        public void Setup()
        {
            _staffServiceMock = new Mock<IStaffService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            _controller = new StaffsController(
                _staffServiceMock.Object,
                _notificationServiceMock.Object,
                _authenticationServiceMock.Object
            );
        }

        [Test]
        public async Task CreateAccountAsync_ReturnsOkResult_WhenAccountIsCreatedSuccessfully()
        {
            // Arrange
            var dto = new StudentCreateAccountRequest(); // Provide necessary data for the request

            // Act
            var result = await _controller.CreateAccountAsync(dto) as ActionResult<EmailContentRequest>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task RequestEmailAsync_ReturnsOkResult_WhenEmailRequestIsSentSuccessfully()
        {
            // Arrange
            int parentId = 1; // Provide necessary data for the request
            string studentName = "TestStudent"; // Provide necessary data for the request

            // Act
            var result = await _controller.RequestEmailAsync(parentId, studentName) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            // Add more assertions if necessary
        }

        [Test]
        public async Task SendEmailAsync_ReturnsOkResult_WhenEmailIsSentSuccessfully()
        {
            // Arrange
            var request = new EmailContentRequest(); // Provide necessary data for the request

            // Act
            var result = await _controller.SendEmailAsync(request) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            // Add more assertions if necessary
        }

        [Test]
        public async Task ViewReasonAsync_ReturnsOkResult_WithCancellationReason()
        {
            // Arrange
            int orderId = 1; // Provide necessary data for the request

            // Act
            var result = await _controller.ViewReasonAsync(orderId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Value);
            // Add more assertions if necessary
        }

        [Test]
        public async Task ViewReasonAsync_ReturnsOkResult_WithReason_WhenOrderIdIsValid()
        {
            // Arrange
            int orderId = 1; // Provide necessary data
            string expectedReason = "Cancellation reason"; // Provide expected reason

            // Mock necessary dependencies
            _authenticationServiceMock.Setup(a => a.CheckAccountStatus());
            _staffServiceMock.Setup(s => s.ViewReasonOrderCancel(orderId)).ReturnsAsync(expectedReason);

            // Act
            var result = await _controller.ViewReasonAsync(orderId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedReason, okResult.Value.GetType().GetProperty("Reason").GetValue(okResult.Value));
        }
    }
}