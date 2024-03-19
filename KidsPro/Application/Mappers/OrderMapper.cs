using Application.Dtos.Response.Account.Student;
using Application.Dtos.Response.Order;
using Application.Utils;
using Domain.Entities;

namespace Application.Mappers
{
    public class OrderMapper
    {
        public static OrderPaymentResponse OrderToOrderPaymentResponse(Order order) => new OrderPaymentResponse()
        {
            ParentId = order.ParentId,
            OrderId = order.Id,
        };

        public static List<OrderResponse> ParentShowOrder(List<Order> orders)
        {
            var list = new List<OrderResponse>();
            foreach (var x in orders)
            {
                var dto = new OrderResponse()
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

        public static OrderDetailResponse ParentShowOrderDetail(Order order)
        {
            var x = new OrderDetailResponse()
            {
                OrderId = order.Id,
                Status = order.Status.ToString(),
                PictureUrl = order.OrderDetails!.FirstOrDefault()!.Course.PictureUrl,
                CourseName = order.OrderDetails!.FirstOrDefault()!.Course.Name,
                Price = order.OrderDetails!.FirstOrDefault()!.Course.Price,
                QuantityPurchased = order.Quantity,
                OrderDate = DateUtils.FormatDateTimeToDatetimeV1(order.Date),
                OrderCode = order.OrderCode,
                PaymentType = order.PaymentType.ToString(),
                TransactionCode = order.Transaction?.TransactionCode,
                ParentEmail = order.Parent!.PhoneNumber,
                ParentZalo = order.Parent!.Account.Email,
                TotalPrice = order.TotalPrice,
                Discount = order.Voucher?.DiscountAmount,
                NumberChildren = order.OrderDetails!.FirstOrDefault()!.Students!.Count,
            };
            foreach (var dto in order.OrderDetails!.FirstOrDefault()!.Students!)
            {
                var stu = new StudentOrderDetail()
                {
                    AcountId = dto.Account.Id,
                    StudentName = dto.Account.FullName
                };
                x.Students?.Add(stu);
            }

            return x;
        }
        
        
    }
}