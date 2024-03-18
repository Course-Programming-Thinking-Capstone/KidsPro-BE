using Application.Dtos.Request.Order;
using Application.Dtos.Response.Order;
using Domain.Enums;

namespace Application.Interfaces.IServices
{
    public interface IOrderService
    {
        Task<OrderPaymentResponseDto> CreateOrderAsync(OrderRequestDto dto);

        Task<bool> StatusToPendingAsync(int orderId, int parentId);

        Task<List<OrderResponseDto>> GetListOrderAsync(OrderStatus status);
    }
}