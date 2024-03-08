using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class SectionComponentNumberRepository : BaseRepository<SectionComponentNumber>,
    ISectionComponentNumberRepository
{
    public SectionComponentNumberRepository(AppDbContext context,
        ILogger<BaseRepository<SectionComponentNumber>> logger) : base(context, logger)
    {
    }

    public async Task<SectionComponentNumber?> GetByTypeAsync(SectionComponentType type)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.SectionComponentType == type);
    }
}