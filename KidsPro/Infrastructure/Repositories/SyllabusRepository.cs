using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class SyllabusRepository : BaseRepository<Syllabus>, ISyllabusRepository
{
    public SyllabusRepository(AppDbContext context, ILogger<BaseRepository<Syllabus>> logger) : base(context, logger)
    {
    }

    public override async Task<Syllabus?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Syllabus> query = _dbSet;

        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await query.Include(s => s.Course)
            .ThenInclude(c => c.Sections)
            .Include(s => s.PassCondition)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}