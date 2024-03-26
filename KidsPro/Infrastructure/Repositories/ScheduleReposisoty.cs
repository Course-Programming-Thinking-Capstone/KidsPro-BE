using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class ScheduleReposisoty:BaseRepository<ClassSchedule>,IScheduleReposisoty
{
    public ScheduleReposisoty(AppDbContext context, ILogger<BaseRepository<ClassSchedule>> logger) : base(context, logger)
    {
    }

    public async Task<List<ClassSchedule>> GetScheduleByClassIdAsync(int classId)
    {
        var query = _dbSet.AsNoTracking();
        return await query.Where(x => x.ClassId == classId)
            .Include(x => x.Class)
            .ThenInclude(x => x.Course).ThenInclude(x => x.Syllabus).ToListAsync();
    }
}