using Application.Dtos.Request.Order.Momo;
using Application.Dtos.Response.Order;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Request.Order.ZaloPay;

namespace Application.Interfaces.IServices
{
    public interface IPaymentService
    {
        Task<Order?> GetOrderStatusPaymentAsync(int orderId);
        string MakeSignatureMomoPayment(string accessKey, string secretKey, MomoPaymentRequest momo);
        (string?, string?) GetLinkGatewayMomo(string paymentUrl, MomoPaymentRequest momoRequest);
        Task<int> CreateTransactionAsync(MomoResultRequest dto);
        string MakeSignatureZaloPayment(string Key, ZaloPaymentRequest zalo);
        string? GetLinkGatewayZaloPay(string paymentUrl, ZaloPaymentRequest zaloRequest);
    }
}
