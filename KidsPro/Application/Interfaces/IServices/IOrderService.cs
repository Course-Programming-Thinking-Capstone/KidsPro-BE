using Application.Dtos.Request.Order;
using Application.Dtos.Response.Order;
using Domain.Enums;

namespace Application.Interfaces.IServices
{
    public interface IOrderService
    {
        Task<OrderPaymentResponse> CreateOrderAsync(OrderRequest dto);

        Task UpdateOrderStatusAsync(int orderId, int parentId,
            OrderStatus currentStatus, OrderStatus toStatus, string? reason="");

        Task<List<OrderResponse>> GetListOrderAsync(OrderStatus status);

        Task<OrderDetailResponse> GetOrderDetail(int orderId);

        Task CanCelOrder(OrderCancelRequest dto);
    }
}