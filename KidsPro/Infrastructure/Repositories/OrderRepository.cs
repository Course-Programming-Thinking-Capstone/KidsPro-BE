using Application.Configurations;
using Application.Dtos.Response.Paging;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context, ILogger<BaseRepository<Order>> logger) : base(context, logger)
        {
        }

        public async Task<(Order?, string?)> GetByOrderCode(Func<int, string> generateOrderCode)
        {
            string orderCode = generateOrderCode(7);

            // tìm kiếm xem order code có tồn tại trong system chưa
            // theo order code để tạo mới order, còn nếu trùng thì trả về 1 order để check rồi tạo orderCode khác
            return (await _dbSet.FirstOrDefaultAsync(x => x.OrderCode == orderCode), orderCode);
        }

        public async Task<Order?> SearchByOrderCode(string code)
        {
            return await _dbSet.Include(x => x.OrderDetails)!
                .ThenInclude(x => x.Course)
                .Include(x=>x.Parent).ThenInclude(x=>x!.Account)
                .FirstOrDefaultAsync(x => x.OrderCode == code);
        }

        public async Task<Order?> GetOrderByStatusAsync(int orderId, OrderStatus status)
        {
            var query = _dbSet;

            return await query.Include(x => x.Parent).ThenInclude(x => x!.Account)
                .FirstOrDefaultAsync(x => x.Id == orderId && x.Status == status);
        }

        public async Task<PagingResponse<Order>> GetListOrderAsync(OrderStatus status, int parentId, string role,
            int pageSize, int pageNumber)
        {
            var result = new PagingResponse<Order>();
            var query = _dbSet.AsNoTracking();
            if (role == Constant.ParentRole)
            {
                query = query.Where(x => x.ParentId == parentId);
            }

            if (status != OrderStatus.AllStatus)
                query = query.Where(x => x.Status == status);

            result.TotalRecords = await query.CountAsync();

            result.Results = await query.Include(x => x.Parent).ThenInclude(x => x!.Account)
                .Include(x => x.OrderDetails)!.ThenInclude(x => x.Course)
                .OrderByDescending(x => x.Date)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            result.TotalPages = (int)Math.Ceiling((double)result.TotalRecords / pageSize);
            return result;
        }

        public async Task<List<Order>?> MobileGetListOrderAsync(OrderStatus status, int parentId, string role)
        {
            var query = _dbSet.AsNoTracking();
            if (role == Constant.ParentRole)
            {
                query = query.Where(x => x.ParentId == parentId);
            }

            if (status != OrderStatus.AllStatus)
                query = query.Where(x => x.Status == status);

            return await query.Include(x => x.Parent).ThenInclude(x => x!.Account)
                .Include(x => x.OrderDetails)!.ThenInclude(x => x.Course)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderDetail(int parentId, int orderId, string role)
        {
            var query = _dbSet.AsNoTracking();
            if (role == Constant.ParentRole)
            {
                query = query.Where(x => x.ParentId == parentId);
            }

            return await query.Include(x => x.OrderDetails)!
                .ThenInclude(x => x.Students).ThenInclude(x => x.Account)
                .Include(x => x.OrderDetails)!.ThenInclude(x => x.Course)
                .Include(x => x.Transaction).Include(x => x.Voucher)
                .Include(x => x.Parent).ThenInclude(x => x!.Account)
                .Include(x => x.OrderDetails)!.ThenInclude(x => x.Class)
                .ThenInclude(x => x.Students)
                .FirstOrDefaultAsync(x => x.Id == orderId);
        }

        public override Task<Order?> GetByIdAsync(int id, bool disableTracking = false)
        {
            return _dbSet.Include(x => x.Parent).ThenInclude(x => x!.Account)
                .Include(x=>x.OrderDetails).ThenInclude(x=>x.Course).ThenInclude(x=>x.Sections)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}