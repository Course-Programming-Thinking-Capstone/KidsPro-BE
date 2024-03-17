using Application.Dtos.Request.Order.Momo;
using Application.Dtos.Response.Order;
using Application.Dtos.Response.Order.Momo;
using Application.Interfaces.IServices;
using Application.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Gateway.IConfig;

namespace WebAPI.Controllers
{
    [Route("api/v1/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        IPaymentService _payment;
        IMomoConfig _momoConfig;

        public PaymentController(IPaymentService payment, IMomoConfig momoConfig)
        {
            _payment = payment;
            _momoConfig = momoConfig;
        }

        /// <summary>
        /// Tạo mã QR Code để thanh toán momo, lấy link
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("momo")]
        public async Task<ActionResult> CreatePaymentMomoAsync(OrderResponseDto dto)
        {
            var _momoRequest = new MomoPaymentRequestDto();
            //Get order có parent id và order id vs status payment
            var _order = await _payment.GetOrderPaymentAsync(dto);
            if (_order != null)
            {
                // Lấy thông tin cho payment
                _momoRequest.requestId = HashingUtils.GenerateRandomString(4) + "-" + dto.ParentId.ToString();
                _momoRequest.orderId = HashingUtils.GenerateRandomString(4) + "-" + dto.OrderId.ToString();
                _momoRequest.amount =(long) _order.TotalPrice;
                //_momoRequest.extraData = DateTime.UtcNow.AddDays(1).ToString();
                _momoRequest.redirectUrl = _momoConfig.ReturnUrl;
                _momoRequest.ipnUrl = _momoConfig.IpnUrl;
                _momoRequest.partnerCode = _momoConfig.PartnerCode;
                _momoRequest.orderInfo = " 'KidsPro Service' - You are payment for " + _order.Note;
                _momoRequest.signature = _payment.MakeSignatureMomoPayment
                (_momoConfig.AccessKey, _momoConfig.SecretKey, _momoRequest);
            }
            // lấy link QR momo
            var result = _payment.GetLinkGatewayMomo(_momoConfig.PaymentUrl, _momoRequest);
            return Ok(new
            {
                payUrl= result.Item1,
                qrCode= result.Item2,
            });
        }

        /// <summary>
        /// Không dùng APi này, sau khi payment success Momo sẽ tự động gọi api return
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("momo-return")]
        public async Task<IActionResult> MomoReturnAsync([FromQuery] MomoResultRequestDto dto)
        {
            await _payment.CreateTransactionAsync(dto);
            return Ok(new
            {
                result = dto.resultCode,
                Message = dto.message,
                PayType=dto.payType
            });
        }

    }
}
