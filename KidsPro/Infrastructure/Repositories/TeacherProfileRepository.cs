using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class TeacherProfileRepository : BaseRepository<TeacherProfile>, ITeacherProfileRepository
{
    public TeacherProfileRepository(AppDbContext context, ILogger<BaseRepository<TeacherProfile>> logger) : base(
        context, logger)
    {
    }

    public Task<List<TeacherProfile>> GetTeacherProfiles(int teacherId)
    {
        return _dbSet.Where(x => x.TeacherId == teacherId).ToListAsync();
    }
}