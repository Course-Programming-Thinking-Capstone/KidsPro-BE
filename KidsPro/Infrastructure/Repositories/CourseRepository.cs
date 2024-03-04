using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context, ILogger<BaseRepository<Course>> logger) : base(context, logger)
    {
    }

    public override async Task<Course?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Course> query = _dbSet;

        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await query.Include(c => c.CreatedBy)
            .Include(c => c.ModifiedBy)
            .Include(c => c.Sections)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDelete);
    }
}