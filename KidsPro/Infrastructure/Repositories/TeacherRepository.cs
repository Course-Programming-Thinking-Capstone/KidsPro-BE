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
    public async Task<List<Teacher>> GetTeacherToClass()
    {
        IQueryable<Teacher> query = _dbSet.AsNoTracking();

        return await query.Include(x=> x.Account)
            //.Include(x => x.Classes).ThenInclude(x => x.Schedules)
            //.Include(x => x.Classes).ThenInclude(x => x.Course)
            .ToListAsync();
    }
    #nullable restore
    
    
}