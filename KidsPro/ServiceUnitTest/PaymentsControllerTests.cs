using Application.Dtos.Request.Order.Momo;
using Application.Dtos.Request.Order.ZaloPay;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Gateway.IConfig;

namespace UnitTest
{
    [TestFixture]
    public class PaymentsControllerTests
    {
        private Mock<IPaymentService> _paymentServiceMock;
        private Mock<IMomoConfig> _momoConfigMock;
        private Mock<IAuthenticationService> _authenticationMock;
        private Mock<IZaloPayConfig> _zaloPayConfigMock;
        private Mock<IOrderService> _orderServiceMock;
        private Mock<IMomoService> _momoServiceMock;
        private PaymentsController _controller;

        [SetUp]
        public void Setup()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _momoConfigMock = new Mock<IMomoConfig>();
            _authenticationMock = new Mock<IAuthenticationService>();
            _zaloPayConfigMock = new Mock<IZaloPayConfig>();
            _orderServiceMock = new Mock<IOrderService>();
            _momoServiceMock = new Mock<IMomoService>();

            _controller = new PaymentsController(
                _paymentServiceMock.Object,
                _momoConfigMock.Object,
                _authenticationMock.Object,
                _zaloPayConfigMock.Object,
                _orderServiceMock.Object,
                _momoServiceMock.Object
            );
        }

       

        [Test]
        public async Task MomoReturnAsync_ReturnsRedirectResult()
        {
            // Arrange
            var dto = new MomoResultRequest();
            var orderId = 1;

            _paymentServiceMock.Setup(service => service.CreateTransactionAsync(dto))
                .ReturnsAsync(orderId);

            // Act
            var result = await _controller.MomoReturnAsync(dto);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

      
    }
}
