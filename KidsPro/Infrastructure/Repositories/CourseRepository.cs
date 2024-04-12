using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context, ILogger<BaseRepository<Course>> logger) : base(context, logger)
    {
    }

    public override async Task<Course?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Course> query = _dbSet;

        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await query.Include(c => c.CreatedBy)
            .Include(c => c.ModifiedBy)
            .Include(c => c.Sections.OrderBy(s => s.Order))
            .ThenInclude(s => s.Lessons.OrderBy(l => l.Order))
            .Include(c => c.Sections.OrderBy(s => s.Order))
            .ThenInclude(s => s.Quizzes.OrderBy(q => q.Order))
            .ThenInclude(q => q.Questions.OrderBy(qu => qu.Order))
            .ThenInclude(q => q.Options.OrderBy(o => o.Order))
            .Include(c => c.Sections.OrderBy(s => s.Order))
            .ThenInclude(s => s.Games)
            .Include(x => x.Syllabus)
            .Include(x => x.Classes).ThenInclude(x => x.Schedules)
            .Include(x => x.Classes).ThenInclude(x => x.Teacher).ThenInclude(x => x!.Account)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDelete);

    }

    public async Task<Course?> GetCoursePayment(int courseId, int classId, bool disableTracking = false)
    {
        IQueryable<Course> query = _dbSet;

        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await query.Include(x => x.Classes).Where(x => x.Classes.Any(c => c.Id == classId))
            .Include(x => x.ModifiedBy).FirstOrDefaultAsync(x => x.Id == courseId);
    }

    public async Task<Course?> CheckCourseExist(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Course>> GetCoursesByStatusAsync(CourseStatus status)
    {
        IQueryable<Course> query = _dbSet.AsNoTracking();

        switch (status)
        {
            case CourseStatus.Pending:
                query = query.Where(x => x.Status == CourseStatus.Pending);
                break;
        }

        return await query.Include(x => x.Sections)
            .Include(x => x.ModifiedBy).ToListAsync();
    }
   

    // public async Task<Course?> GerCourseDetailActive(int courseId)
    // {
    //     IQueryable<Course> query = _dbSet.AsNoTracking();
    //
    //     return  query
    //         .Include(c => c.Sections.OrderBy(s => s.Order))
    //         .ThenInclude(s => s.Lessons.OrderBy(l => l.Order))
    //         .Include(c => c.Sections.OrderBy(s => s.Order))
    //         .ThenInclude(s => s.Quizzes.OrderBy(q => q.Order))
    //         .ThenInclude(q => q.Questions.OrderBy(qu => qu.Order))
    //         .ThenInclude(q => q.Options.OrderBy(o => o.Order))
    //         .Include(c => c.Sections.OrderBy(s => s.Order))
    //         .ThenInclude(s => s.Games)
    //         //  .Include(x => x.Syllabus)
    //         .Include(x => x.Classes).ThenInclude(x => x.Schedules)
    //         .Include(x => x.Classes).ThenInclude(x => x.Teacher).ThenInclude(x => x!.Account)
    //      
    //     .FirstOrDefaultAsync(c => c.Item1.Id == courseId && !c.Item1.IsDelete && c.Item1.Status==CourseStatus.Active);
    //
    // }
    //  .Where(c => c.Id == courseId)
    // .Select(c => new Tuple<Course, List<Section>, List<LessonDto>>(
    // c,
    // c.Sections.ToList(),
    // c.Sections.SelectMany(s => s.Lessons.Select(l => new LessonDto()
    // {
    //     Id = l.Id,
    //     Name = l.Name,
    //     Duration = l.Duration,
    //     Order = l.Order,
    //     Type = l.Type.ToString()
    // })).ToList()
    //     ))

}