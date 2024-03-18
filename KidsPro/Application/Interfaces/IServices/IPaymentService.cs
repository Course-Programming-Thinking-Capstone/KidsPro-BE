using Application.Dtos.Request.Order.Momo;
using Application.Dtos.Response.Order;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IPaymentService
    {
        Task<Order?> GetOrderPaymentAsync(OrderPaymentResponseDto dto);
        string MakeSignatureMomoPayment(string accessKey, string secretKey, MomoPaymentRequestDto momo);
        (string?, string?) GetLinkGatewayMomo(string paymentUrl, MomoPaymentRequestDto momoRequest);
        Task CreateTransactionAsync(MomoResultRequestDto dto);
    }
}
