using Application.Dtos.Request.Order;
using Application.Dtos.Response.Order;
using Domain.Enums;

namespace Application.Interfaces.IServices
{
    public interface IOrderService
    {
        Task<OrderPaymentResponse> CreateOrderAsync(OrderRequest dto);

        Task<bool> StatusToPendingAsync(int orderId, int parentId);

        Task<List<OrderResponse>> GetListOrderAsync(OrderStatus status);

        Task<OrderDetailResponse> GetOrderDetail(int orderId);
    }
}