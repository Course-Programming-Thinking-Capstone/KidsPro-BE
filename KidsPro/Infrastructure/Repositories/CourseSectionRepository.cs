using System.Linq.Expressions;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CourseSectionRepository : BaseRepository<CourseSection>, ICourseSectionRepository
{
    public CourseSectionRepository(AppDbContext context, ILogger<BaseRepository<CourseSection>> logger) : base(context,
        logger)
    {
    }

    public async Task DeleteByCourseIdAsync(int courseId)
    {
        await _dbSet.Where(cs => cs.CourseId == courseId).ExecuteDeleteAsync();
    }

    public async Task IncreaseSectionOrderAsync(int courseId, int order)
    {
        await _dbSet.Where(cs => cs.CourseId == courseId && cs.Order > order)
            .ExecuteUpdateAsync(setters => setters.SetProperty(cs => cs.Order, cs => cs.Order + 1));
    }

    public async Task DecreaseSectionOrderAsync(int courseId, int order)
    {
        await _dbSet.Where(cs => cs.CourseId == courseId && cs.Order > order)
            .ExecuteUpdateAsync(setters => setters.SetProperty(cs => cs.Order, cs => cs.Order - 1));
    }

    public async Task<CourseSection?> GetFirstByFilterAsync(Expression<Func<CourseSection, bool>>? filter, Func<IQueryable<CourseSection>, IOrderedQueryable<CourseSection>> orderBy, string? includeProperties = null,
        bool disableTracking = false)
    {
        IQueryable<CourseSection> query = _dbSet;

        try
        {
            if (disableTracking)
            {
                query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            return await query.FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error when filter data of {} entity.", typeof(CourseSection));
            throw;
        }
    }

    public override async Task<bool> ExistById(int id)
    {
        return await _dbSet.AnyAsync(cs => cs.Id == id);
    }
}