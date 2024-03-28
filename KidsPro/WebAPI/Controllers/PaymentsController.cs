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
        private IAuthenticationService _authentication;

        public PaymentsController(IPaymentService payment, IMomoConfig momoConfig,
            IAuthenticationService authentication)
        {
            _payment = payment;
            _momoConfig = momoConfig;
            _authentication = authentication;
        }

        /// <summary>
        /// Tạo link url payment Momo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("momo/{id}")]
        public async Task<ActionResult> CreatePaymentMomoAsync(int id)
        {
            //Check if the account is activated or not or inactive
            _authentication.CheckAccountStatus();

            var momoRequest = new MomoPaymentRequest();
            //Get order có parent id và order id vs status payment
            var order = await _payment.GetOrderStatusPaymentAsync(id);
            if (order == null) return BadRequest($"OrderID:{id} doesn't not exist");
            
            // Lấy thông tin cho payment
            momoRequest.requestId = StringUtils.GenerateRandomString(4) + "-" + order.ParentId;
            momoRequest.orderId = StringUtils.GenerateRandomString(4) + "-" + order.Id;
            momoRequest.amount = (long)order.TotalPrice;
            momoRequest.redirectUrl = _momoConfig.ReturnUrl;
            momoRequest.ipnUrl = _momoConfig.IpnUrl;
            momoRequest.partnerCode = _momoConfig.PartnerCode;
            momoRequest.orderInfo = " 'KidsPro Service' - You are payment for " + order.Note;
            momoRequest.signature = _payment.MakeSignatureMomoPayment
                (_momoConfig.AccessKey, _momoConfig.SecretKey, momoRequest);
            
            // lấy link QR momo
            var result = _payment.GetLinkGatewayMomo(_momoConfig.PaymentUrl, momoRequest);
            return Ok(new
            {
                payUrl = result.Item1,
                qrCode = result.Item2,
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
            return Redirect($"https://capstone-one-rose.vercel.app/payment-success/{dto.orderId}");
        }
    }
}