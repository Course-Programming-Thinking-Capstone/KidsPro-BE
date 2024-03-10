using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class SectionRepository : BaseRepository<Section>, ISectionRepository
{
    public SectionRepository(AppDbContext context, ILogger<BaseRepository<Section>> logger) : base(context, logger)
    {
    }

    public async Task<bool> ExistByOrderAsync(int courseId, int order)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.CourseId == courseId && s.Order == order)
            .ContinueWith(t => t.Result != null);
    }

    public override async Task<Section?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Section> query = _dbSet;

        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await query.Include(s => s.Lessons.Where(l => !l.IsDelete).OrderBy(l => l.Order))
            .Include(s => s.Quizzes.OrderBy(q => q.Order))
            .Include(s => s.Games)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}