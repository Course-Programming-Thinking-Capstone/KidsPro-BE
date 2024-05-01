using Application.Dtos.Response.Notification;
using Application.Dtos.Response.Paging;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace UnitTest
{
    [TestFixture]
    public class NotificationsControllerTests
    {
        private NotificationsController _controller;
        private Mock<INotificationService> _notificationServiceMock;

        [SetUp]
        public void Setup()
        {
            _notificationServiceMock = new Mock<INotificationService>();
            _controller = new NotificationsController(_notificationServiceMock.Object);
        }

        [Test]
        public async Task GetAccountNotificationAsync_ReturnsOkResult()
        {
            // Arrange
            int? page = 1;
            int? size = 10;
            var expectedResponse = new PagingResponse<NotificationDto>();

            _notificationServiceMock.Setup(s => s.GetAccountNotificationAsync(page, size)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAccountNotificationAsync(page, size);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.AreEqual(expectedResponse, (result.Result as OkObjectResult)?.Value);
        }

        [Test]
        public async Task GetNumberOfAccountUnreadNotificationAsync_ReturnsOkResult()
        {
            // Arrange
            var expectedResponse = 5;

            _notificationServiceMock.Setup(s => s.GetNumberOfAccountUnreadNotificationAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetNumberOfAccountUnreadNotificationAsync();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.AreEqual(expectedResponse, (result.Result as OkObjectResult)?.Value);
        }

        [Test]
        public async Task MarkNotificationAsReadAsync_ReturnsOkResult()
        {
            // Arrange
            int id = 1;
            var expectedResponse = new NotificationDto();

            _notificationServiceMock.Setup(s => s.MarkNotificationAsReadAsync(id)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.MarkNotificationAsReadAsync(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.AreEqual(expectedResponse, (result.Result as OkObjectResult)?.Value);
        }

        [Test]
        public async Task MarkAllNotificationAsReadAsync_ReturnsOkResult()
        {
            // Arrange
            int? page = 1;
            int? size = 10;
            var expectedResponse = new PagingResponse<NotificationDto>();

            _notificationServiceMock.Setup(s => s.MarkAllNotificationAsReadAsync(page, size)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.MarkAllNotificationAsReadAsync(page, size);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.AreEqual(expectedResponse, (result.Result as OkObjectResult)?.Value);
        }
                [Test]
        public async Task GetAccountNotificationAsync_ReturnsCorrectPagingResponse()
        {
            // Arrange
            var expectedResponse = new PagingResponse<NotificationDto>();
            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(s => s.GetAccountNotificationAsync(It.IsAny<int?>(), It.IsAny<int?>()))
                                   .ReturnsAsync(expectedResponse);
            var controller = new NotificationsController(notificationServiceMock.Object);

            // Act
            var result = await controller.GetAccountNotificationAsync(page: 1, size: 10);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }

        [Test]
        public async Task GetNumberOfAccountUnreadNotificationAsync_ReturnsCorrectNumberOfUnreadNotifications()
        {
            // Arrange
            const int expectedUnreadCount = 5;
            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(s => s.GetNumberOfAccountUnreadNotificationAsync())
                                   .ReturnsAsync(expectedUnreadCount);
            var controller = new NotificationsController(notificationServiceMock.Object);

            // Act
            var result = await controller.GetNumberOfAccountUnreadNotificationAsync();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedUnreadCount, okResult?.Value);
        }

        [Test]
        public async Task MarkNotificationAsReadAsync_ReturnsCorrectNotificationDto()
        {
            // Arrange
            var expectedNotificationDto = new NotificationDto();
            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(s => s.MarkNotificationAsReadAsync(It.IsAny<int>()))
                                   .ReturnsAsync(expectedNotificationDto);
            var controller = new NotificationsController(notificationServiceMock.Object);

            // Act
            var result = await controller.MarkNotificationAsReadAsync(id: 1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedNotificationDto, okResult?.Value);
        }

        [Test]
        public async Task MarkAllNotificationAsReadAsync_ReturnsCorrectPagingResponse()
        {
            // Arrange
            var expectedResponse = new PagingResponse<NotificationDto>();
            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(s => s.MarkAllNotificationAsReadAsync(It.IsAny<int?>(), It.IsAny<int?>()))
                                   .ReturnsAsync(expectedResponse);
            var controller = new NotificationsController(notificationServiceMock.Object);

            // Act
            var result = await controller.MarkAllNotificationAsReadAsync(page: 1, size: 10);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(expectedResponse, okResult?.Value);
        }

    }
}
