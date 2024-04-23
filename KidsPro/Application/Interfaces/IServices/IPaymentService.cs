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
        Task<int> CreateTransactionAsync(MomoResultRequest dto);
        string MakeSignatureZaloPayment(string key, ZaloPaymentRequest zalo);

        string? GetLinkGatewayZaloPay(string paymentUrl, ZaloPaymentRequest zaloRequest);

        // Task<Transaction> GetTransactionByOrderIdAsync(int orderId);
        Task<MomoPaymentResponse> RequestMomoRefundAsync(int orderId);
        Task UpdateTransStatusToRefunded(string momoOrderId);
    }
}