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
    public class VoucherRepository : BaseRepository<GameVoucher>, IVoucherRepository
    {
        public VoucherRepository(AppDbContext context, ILogger<BaseRepository<GameVoucher>> logger) : base(context, logger)
        {
        }

        public async Task<GameVoucher?> GetVoucher(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id &&
            x.Status == VoucherStatus.Valid && (x.ExpiredDate <= DateTime.UtcNow));
        }
    }
}
