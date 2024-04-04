using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context, ILogger<BaseRepository<Transaction>> logger) : base(context, logger)
        {
        }

        public override Task<Transaction?> GetByIdAsync(int id, bool disableTracking = false)
        {
            return _dbSet.Include(x => x.Order).ThenInclude(x=> x!.Parent)
                .FirstOrDefaultAsync(x => x.OrderId == id && x.Status==TransactionStatus.Success);
        }
       
    }
