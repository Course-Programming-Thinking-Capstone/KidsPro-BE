using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Constant = Application.Configurations.Constant;

namespace Infrastructure.Repositories;

public class ClassRepository : BaseRepository<Class>, IClassRepository
{
    public ClassRepository(AppDbContext context, ILogger<BaseRepository<Class>> logger) : base(context, logger)
    {
    }

    public async Task<bool> ExistByClassCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Code == code)
            .ContinueWith(task => task.Result != null);
    }

    public override Task<Class?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Class> query = _dbSet;
        return query.Include(x => x.Schedules)
            .Include(x => x.Teacher).ThenInclude(x => x!.Account)
            .Include(x => x.Course).ThenInclude(x => x.Syllabus)
            .Include(x => x.Students).ThenInclude(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Class>> GetClassByRoleAsync(int id, string role)
    {
        IQueryable<Class> query = _dbSet.AsNoTracking();

        switch (role)
        {
            case Constant.TeacherRole:
                query = query.Where(x => x.TeacherId == id);
                break;
            case Constant.StudentRole:
                query = query.Include(x => x.Students)
                    .Where(x => x.Students.Any(s => s.Id == id && x.Students.Count > 0));
                break;
        }

        return await query.Where(x => x.Status == ClassStatus.OnGoing)
            .Include(x => x.Course).ThenInclude(x=> x.Syllabus)
            .Include(x => x.Teacher).ThenInclude(x => x!.Account)
            .Include(x => x.Schedules).ToListAsync();
    }

    public async Task<Class?> GetClassByCodeAsync(string code)
    {
        return await _dbSet.Where(c => c.Code == code)
            .Include(c => c.Students.OrderBy(s => s.Account.FullName))
            .ThenInclude(s => s.Account)
            .Include(c => c.Course)
            .FirstOrDefaultAsync();
    }
}