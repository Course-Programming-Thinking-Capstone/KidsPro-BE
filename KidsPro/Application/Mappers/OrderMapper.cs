using Application.Dtos.Response.Order;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class OrderMapper
    {
        public static OrderResponseDto OrderToOrderResponse(Order order) => new OrderResponseDto()
        {
            OrderId = order.Id,
            Amount = order.TotalPrice
        };
    }
}
