using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class PassConditionRepository : BaseRepository<PassCondition>, IPassConditionRepository
{
    public PassConditionRepository(AppDbContext context, ILogger<BaseRepository<PassCondition>> logger) : base(context,
        logger)
    {
    }

    public async Task<PassCondition?> GetByPassRatioAsync(int passRatio, bool disableTracking = false)
    {
        IQueryable<PassCondition> query = _dbSet;
        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(p => p.PassRatio == passRatio);
    }
}