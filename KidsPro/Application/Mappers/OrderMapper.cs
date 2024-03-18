using Application.Dtos.Response.Order;
using Domain.Entities;

namespace Application.Mappers
{
    public class OrderMapper
    {
        public static OrderPaymentResponseDto OrderToOrderPaymentResponse(Order order) => new OrderPaymentResponseDto()
        {
            ParentId = order.ParentId,
            OrderId = order.Id,
        };

        public static List<OrderResponseDto> ParentShowOrder(List<Order> orders)
        {
            var list = new List<OrderResponseDto>();
            foreach (var x in orders)
            {
                var dto = new OrderResponseDto()
                {
                    OrderId = x.Id,
                    OrderCode = x.OrderCode,
                    CourseName = x.OrderDetails!.FirstOrDefault()?.Course.Name,
                    PictureUrl = x.OrderDetails!.FirstOrDefault()?.Course.PictureUrl,
                    Quantity = x.Quantity,
                    TotalPrice = x.TotalPrice,
                    OrderStatus = x.Status.ToString()
                };
                list.Add(dto);
            }

            return list;
        }
        
    }
}