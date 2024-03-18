using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(AppDbContext context, ILogger<BaseRepository<OrderDetail>> logger) : base(context, logger)
        {
        }
    }
}
