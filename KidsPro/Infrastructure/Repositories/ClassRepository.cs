using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class ClassRepository : BaseRepository<Class>, IClassRepository
{
    public ClassRepository(AppDbContext context, ILogger<BaseRepository<Class>> logger) : base(context, logger)
    {
    }

    public async Task<bool> ExistByClassCode(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Code == code)
            .ContinueWith(task => task.Result != null);
    }

    public override Task<Class?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Class> query = _dbSet.AsNoTracking();
        return query.Include(x => x.Schedules)
            .Include(x => x.Teacher).ThenInclude(x => x.Account)
            .Include(x => x.Course).ThenInclude(x=> x.Syllabus)
            .Include(x => x.Students).FirstOrDefaultAsync(x => x.Id == id);
    }
}