using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.ErrorHandlers;
using static Domain.Enums.VoucherStatus;

namespace Infrastructure.Repositories
{
    public class VoucherRepository : BaseRepository<GameVoucher>, IVoucherRepository
    {
        public VoucherRepository(AppDbContext context, ILogger<BaseRepository<GameVoucher>> logger) : base(context,
            logger)
        {
        }

        public async Task<GameVoucher?> GetVoucherPaymentAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id &&
                                                         x.Status == Valid && (x.ExpiredDate >= DateTime.UtcNow));
        }

        public async Task<List<GameVoucher>?> GetListVoucher(int parentId, VoucherStatus status)
        {
            IQueryable<GameVoucher> query = _dbSet.AsNoTracking();
            switch (status)
            {
                case Valid:
                    query = query.Where(x => x.ParentId == parentId &&
                                             x.Status == Valid && (x.ExpiredDate >= DateTime.UtcNow));
                    break;
                case Expired:
                    query = query.Where(x => x.ParentId == parentId &&
                                             x.Status == Expired && (x.ExpiredDate <= DateTime.UtcNow));
                    break;
                case Used:
                    query = query.Where(x => x.ParentId == parentId &&
                                             x.Status == Used );
                    break;
                case AllStatus:
                    query = query.Where(x => x.ParentId == parentId);
                    break;
            }

            return await query.ToListAsync();
        }
    }
}