using Application.Dtos.Request.Order;
using Application.Dtos.Request.Order.Momo;
using Application.Dtos.Response.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto dto);

        Task<bool> StatusToPendingAsync(int orderId, int parentId);
    }
}
