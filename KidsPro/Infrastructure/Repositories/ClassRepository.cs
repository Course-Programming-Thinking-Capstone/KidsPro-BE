using Application.Interfaces.IRepositories;
using Domain.Entities;
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

    public async Task<bool> ExistByClassCode(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Code == code)
            .ContinueWith(task => task.Result != null);
    }

    public override Task<Class?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Class> query = _dbSet;
        return query.Include(x => x.Schedules)
            .Include(x => x.Teacher).ThenInclude(x => x!.Account)
            .Include(x => x.Course).ThenInclude(x=> x.Syllabus)
            .Include(x => x.Students).ThenInclude(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    
    // public Task<IEnumerable<Class>> GetAsync(Expression<Func<Class, bool>>? filter, Func<IQueryable<Class>, IOrderedQueryable<Class>>? orderBy, string? includeProperties = null, bool disableTracking = false)
    // {
    //     throw new NotImplementedException();
    // }

    public async Task<List<Class>> GetClassByRole(int id,string role)
    {
        IQueryable<Class> query = _dbSet.AsNoTracking();

        switch (role)
        {
            case Constant.TeacherRole:
                query = query.Where(x => x.TeacherId == id);
                break;
            case Constant.StudentRole:
                query = query.Include(x => x.Students)
                    .Where(x => x.Students.All(s => s.Id == id));
                break;

        }

        return await query.Include(x=>x.Course)
            .Include(x=>x.Teacher).ThenInclude(x=>x!.Account)
            .Include(x => x.Schedules).ToListAsync();
    }
}