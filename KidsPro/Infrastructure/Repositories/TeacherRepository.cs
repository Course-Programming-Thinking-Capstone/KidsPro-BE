using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext context, ILogger<BaseRepository<Teacher>> logger) : base(context, logger)
    {
    }

#nullable disable
    public async Task<List<Teacher>> GetTeacherSchedules()
    {
        IQueryable<Teacher> query = _dbSet.AsNoTracking();

        return await query.Include(x => x.Account)
            .Include(x => x.Classes).ThenInclude(x => x.Schedules)
            .Include(x => x.Classes).ThenInclude(x => x.Course)
            .ToListAsync();
    }

    public async Task<Teacher> GetTeacherSchedulesById(int teacherId)
    {
        IQueryable<Teacher> query = _dbSet.AsNoTracking();

        return await query.Include(x => x.Account)
            .Include(x => x.Classes).ThenInclude(x => x.Schedules)
            .FirstOrDefaultAsync(x => x.Id == teacherId);
    }
#nullable restore

    public override Task<Teacher?> GetByIdAsync(int id, bool disableTracking = false)
    {
        return _dbSet.Include(x => x.Account).Include(x=> x.TeacherProfiles)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}