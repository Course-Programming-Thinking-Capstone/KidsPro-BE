using Application.Dtos.Request.Order;
using Application.Dtos.Response.Order;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.IServices
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(OrderRequest dto);

        Task UpdateOrderStatusAsync(int orderId,OrderStatus currentStatus, OrderStatus toStatus, string? reason="");

        Task<PagingOrderResponse> GetListOrderAsync(OrderStatus status, int? pageSize, int? pageNumber);

        Task<OrderDetailResponse> GetOrderDetail(int orderId);

        Task ParentCanCelOrderAsync(OrderCancelRequest dto);
        Task HandleRefundRequest(OrderRefundRequest dto, ModerationStatus status);
        Task<Order?> GetOrderByStatusAsync(int orderId, OrderStatus status);
        Task<List<OrderResponse>> MobileGetListOrderAsync(OrderStatus status);
        Task<OrderResponse?> SearchOrderByCodeAsync(string code);
    }
}