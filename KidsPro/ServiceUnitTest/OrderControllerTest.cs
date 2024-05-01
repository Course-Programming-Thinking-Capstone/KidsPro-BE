using Application.Dtos.Request.Order;
using Application.Dtos.Response.Order;
using Application.Interfaces.IServices;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;

namespace UnitTest
{
    public class OrderControllerTests
    {
        private Mock<IOrderService> _mockOrderService;
        private Mock<IAuthenticationService> _mockAuthenService;
        private OrderController _controller;

        [SetUp]
        public void Setup()
        {
            _mockOrderService = new Mock<IOrderService>();
            _mockAuthenService = new Mock<IAuthenticationService>();
            _controller = new OrderController(_mockOrderService.Object, _mockAuthenService.Object);
        }

        [Test]
        public async Task CreateOrderAsync_ReturnsCreatedStatusCode_WhenOrderIsCreated()
        {
            // Arrange
            var orderRequestDto = new OrderRequest();
            var orderId = 1;

            _mockOrderService.Setup(s => s.CreateOrderAsync(It.IsAny<OrderRequest>())).ReturnsAsync(orderId);

            // Act
            var result = await _controller.CreateOrderAsync(orderRequestDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var createdResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, createdResult.StatusCode);
        }

        [Test]
        public async Task CreateOrderAsync_ReturnsOkResult_WhenOrderIsCreated()
        {
            // Arrange
            var orderRequestDto = new OrderRequest();
            var orderId = 1;

            _mockOrderService.Setup(s => s.CreateOrderAsync(It.IsAny<OrderRequest>())).ReturnsAsync(orderId);

            // Act
            var result = await _controller.CreateOrderAsync(orderRequestDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Test]
        public async Task CreateOrderAsync_ReturnsOkResult_WhenOrderIsCreatedSuccessfully()
        {
            // Arrange
            var orderRequest = new OrderRequest(); // Populate with sample data
            var orderId = 1;

            _mockOrderService.Setup(s => s.CreateOrderAsync(orderRequest)).ReturnsAsync(orderId);

            // Act
            var result = await _controller.CreateOrderAsync(orderRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Test]
        public async Task CanCelOrderAsync_ReturnsOkResult_WhenOrderCancellationRequestIsSentSuccessfully()
        {
            // Arrange
            var orderCancelRequest = new OrderCancelRequest(); // Populate with sample data

            // Act
            var result = await _controller.CanCelOrderAsync(orderCancelRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual("Order cancellation request sent successfully", okResult.Value);
        }

        [Test]
        public async Task ApproveOrderCancellationAsync_ReturnsOkResult_WhenRefundRequestIsHandledSuccessfully()
        {
            // Arrange
            var orderRefundRequest = new OrderRefundRequest(); // Populate with sample data
            var status = ModerationStatus.Approve;

            // Act
            var result = await _controller.ApproveOrderCancellationAsync(orderRefundRequest, status);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual("Handling the request successfully", okResult.Value);
        }

        [Test]
        public async Task ConfirmOrderAsync_ReturnsOkResult_WhenOrderIsUpdatedToSuccessStatus()
        {
            // Arrange
            var orderId = 1;

            // Act
            var result = await _controller.ConfirmOrderAsync(orderId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual("Successfully update to success status", okResult.Value);
        }

        [Test]
        public async Task SearchOrderByCodeAsync_ReturnsOkResult_WhenOrderIsFound()
        {
            // Arrange
            var orderCode = "ABC123";
            var orderResponse = new OrderResponse(); // Populate with sample data

            _mockOrderService.Setup(s => s.SearchOrderByCodeAsync(orderCode)).ReturnsAsync(orderResponse);

            // Act
            var result = await _controller.SearchOrderByCodeAsync(orderCode);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(orderResponse, okResult.Value);
        }
    }
}