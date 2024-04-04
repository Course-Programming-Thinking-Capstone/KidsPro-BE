using Application.Dtos.Request.Order.Momo;
using Application.Dtos.Response.Order;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Request.Order.ZaloPay;
using Application.Dtos.Response.Order.Momo;

namespace Application.Interfaces.IServices
{
    public interface IPaymentService
    {
        string MakeSignatureMomoPayment(string accessKey, string secretKey, MomoPaymentRequest momo);
        (string?, string?) GetLinkMomoGateway(string paymentUrl, MomoPaymentRequest momoRequest);
        Task<int> CreateTransactionAsync(MomoResultRequest dto);
        string MakeSignatureZaloPayment(string key, ZaloPaymentRequest zalo);
        string? GetLinkGatewayZaloPay(string paymentUrl, ZaloPaymentRequest zaloRequest);
        Task<Transaction> GetTransactionByOrderIdAsync(int orderId);
        MomoPaymentResponse SentRequestMomoRefund(string paymentUrl, MomoRefundRequest momoRequest);
        string MakeSignatureMomoRefund(string accessKey, string secretKey, MomoRefundRequest momo);
         Task<MomoPaymentResponse> RequestMomoRefundAsync(int orderId);
         Task UpdateTransStatusToRefunded(string momoOrderId);
    }
}
