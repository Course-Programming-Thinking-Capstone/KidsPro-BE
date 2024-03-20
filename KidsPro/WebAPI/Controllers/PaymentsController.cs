using Application.Dtos.Request.Order.Momo;
using Application.Dtos.Response.Order;
using Application.Interfaces.IServices;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Gateway.IConfig;

namespace WebAPI.Controllers
{
    [Route("api/v1/payment")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        IPaymentService _payment;
        IMomoConfig _momoConfig;

        public PaymentsController(IPaymentService payment, IMomoConfig momoConfig)
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
        public async Task<ActionResult> CreatePaymentMomoAsync(OrderPaymentResponse dto)
        {
            var momoRequest = new MomoPaymentRequest();
            //Get order có parent id và order id vs status payment
            var order = await _payment.GetOrderStatusPaymentAsync(dto);
            if (order != null)
            {
                // Lấy thông tin cho payment
                momoRequest.requestId = StringUtils.GenerateRandomString(4) + "-" + dto.ParentId.ToString();
                momoRequest.orderId = StringUtils.GenerateRandomString(4) + "-" + dto.OrderId.ToString();
                momoRequest.amount =(long) order.TotalPrice;
                //_momoRequest.extraData = DateTime.UtcNow.AddDays(1).ToString();
                momoRequest.redirectUrl = _momoConfig.ReturnUrl;
                momoRequest.ipnUrl = _momoConfig.IpnUrl;
                momoRequest.partnerCode = _momoConfig.PartnerCode;
                momoRequest.orderInfo = " 'KidsPro Service' - You are payment for " + order.Note;
                momoRequest.signature = _payment.MakeSignatureMomoPayment
                (_momoConfig.AccessKey, _momoConfig.SecretKey, momoRequest);
            }
            // lấy link QR momo
            var result = _payment.GetLinkGatewayMomo(_momoConfig.PaymentUrl, momoRequest);
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
        public async Task<IActionResult> MomoReturnAsync([FromQuery] MomoResultRequest dto)
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
