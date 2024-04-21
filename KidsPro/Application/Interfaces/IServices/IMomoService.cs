using Application.Dtos.Request.Order.Momo;
using Application.Dtos.Response.Order.Momo;

namespace Application.Interfaces.IServices;

public interface IMomoService
{
    MomoPaymentResponse SentRequestMomoRefund(string paymentUrl, MomoRefundRequest momoRequest);
    (string?, string?) GetLinkMomoGateway(string paymentUrl, MomoPaymentRequest momoRequest);
    string MakeSignatureMomoPayment(string accessKey, string secretKey, MomoPaymentRequest momo);
    string MakeSignatureMomoRefund(string accessKey, string secretKey, MomoRefundRequest momo);
}