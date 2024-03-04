using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class SectionRepository:BaseRepository<Section>, ISectionRepository
{
    public SectionRepository(AppDbContext context, ILogger<BaseRepository<Section>> logger) : base(context, logger)
    {
    }

    public async Task<bool> ExistByOrderAsync(int courseId, int order)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.CourseId == courseId && s.Order == order)
            .ContinueWith(t => t.Result != null);
    }
}