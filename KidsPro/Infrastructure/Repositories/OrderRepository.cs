using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context, ILogger<BaseRepository<Order>> logger) : base(context, logger)
        {
        }

        public async Task<(Order?,string?)> GetByOrderCode(Func<int,string> GenerateOrderCode, bool decision)
        {
            string orderCode = GenerateOrderCode(13);
           
            switch (decision)
            {
                case true:
                    return (await _dbSet.Include(x=> x.OrderDetails).ThenInclude(x=> x.Course)
                            .FirstOrDefaultAsync(x=> x.OrderCode == orderCode),null);
                // tìm kiếm xem order code có tồn tại trong system chưa, truyền vào false, nếu mà tìm không có bằng null thì trả về 
                // theo order code để tạo mới, còn nếu trùng thì trả về 1 order để check rồi tạo orderCode khác
                case false:
                    return (await _dbSet.FirstOrDefaultAsync(x => x.OrderCode == orderCode),orderCode);
            }
        }

        public async Task<Order?> GetOrderPaymentAsync(int parentId, int orderId)
        {
            return await _dbSet.FirstOrDefaultAsync
                (x=> x.Id == orderId && x.ParentId==parentId && x.Status == OrderStatus.Payment);
        }
    }
}
