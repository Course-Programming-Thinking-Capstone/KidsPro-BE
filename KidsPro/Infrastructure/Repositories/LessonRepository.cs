using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class LessonRepository : BaseRepository<Lesson>, ILessonRepository
{
    public LessonRepository(AppDbContext context, ILogger<BaseRepository<Lesson>> logger) : base(context, logger)
    {
    }

    public Task<bool> ExistBySectionIdAndOrder(int sectionId, int order)
    {
        throw new NotImplementedException();
    }

    public override async Task<Lesson?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Lesson> query = _dbSet;

        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(l => l.Id == id && !l.IsDelete);
    }
}