using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Interfaces.IRepositories
{
    public interface IOrderRepository:IBaseRepository<Order>
    {
        Task<(Order?, string?)> GetByOrderCode(Func<int, string> generateOrderCode, bool decision);
        Task<Order?> GetOrderByStatusAsync( int orderId,OrderStatus status);

        Task<List<Order>?> GetListOrderAsync(OrderStatus status, int parentId,string role);
        Task<Order?> GetOrderDetail(int parentId, int orderId, string role);
    }
}
