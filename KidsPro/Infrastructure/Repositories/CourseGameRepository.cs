using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums.Status;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CourseGameRepository : BaseRepository<CourseGame>, ICourseGameRepository
{
    public CourseGameRepository(AppDbContext context, ILogger<BaseRepository<CourseGame>> logger) : base(context,
        logger)
    {
    }

    public async Task<List<CourseGame>> GetAvailableCourseGameAsync()
    {
        // return await _dbSet.Where(cg => !cg.IsDelete && cg.Status == CourseGameStatus.Active && cg.CourseId == null)
        //     .OrderBy(cg => cg.Name)
        //     .ToListAsync();
        return await _dbSet.Where(cg => !cg.IsDelete && cg.Status == CourseGameStatus.Active)
            .OrderBy(cg => cg.Name)
            .ToListAsync();
    }
}