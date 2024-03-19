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
        public VoucherRepository(AppDbContext context, ILogger<BaseRepository<GameVoucher>> logger) : base(context, logger)
        {
        }

        public async Task<GameVoucher?> GetVoucher(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id &&
            x.Status == Valid && (x.ExpiredDate <= DateTime.UtcNow));
        }
        
        public async Task<List<GameVoucher>?> GetListVoucher(int parentId, VoucherStatus status)
        {
            switch (status)
            {
                case Valid:
                    return await _dbSet.Where(x => x.ParentId == parentId &&
                                                   x.Status == Valid && (x.ExpiredDate <= DateTime.UtcNow)).ToListAsync();
                case Expired:
                    return await _dbSet.Where(x => x.ParentId == parentId &&
                                                   x.Status == Expired && (x.ExpiredDate > DateTime.UtcNow)).ToListAsync();
                case Used:
                    return await _dbSet.Where(x => x.ParentId == parentId &&
                                                   x.Status == Used && (x.ExpiredDate <= DateTime.UtcNow)).ToListAsync();
            }
            throw new BadRequestException("Get list voucher error");
        }
        
    }
}
