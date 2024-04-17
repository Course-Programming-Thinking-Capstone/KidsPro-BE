using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class SectionRepository : BaseRepository<Section>, ISectionRepository
{
    public SectionRepository(AppDbContext context, ILogger<BaseRepository<Section>> logger) : base(context, logger)
    {
    }

    public async Task<bool> ExistByOrderAsync(int courseId, int order)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.CourseId == courseId && s.Order == order)
            .ContinueWith(t => t.Result != null);
    }

    public async Task<Section?> GetStudySectionById(int id, List<CourseStatus> courseStatuses)
    {
        return await _dbSet.Where(s => s.Id == id && !s.Course.IsDelete && courseStatuses.Contains(s.Course.Status) )
            .Select(s => new Section()
            {
                Id = s.Id,
                Name = s.Name,
                SectionTime = s.SectionTime,
                Lessons = s.Lessons.Select(lesson => new Lesson()
                {
                    Id = lesson.Id,
                    Name = lesson.Name,
                    Duration = lesson.Duration,
                    Type = lesson.Type,
                    IsFree = lesson.IsFree
                }).ToList(),
                Quizzes = s.Quizzes.Select(quiz => new Quiz()
                {
                    Id = quiz.Id,
                    TotalQuestion = quiz.TotalQuestion,
                    Duration = quiz.Duration,
                    Title = quiz.Title
                }).ToList()
            }).FirstOrDefaultAsync();
    }

    public override async Task<Section?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Section> query = _dbSet;

        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await query.Include(s => s.Lessons.OrderBy(l => l.Order))
            .Include(s => s.Quizzes.OrderBy(q => q.Order))
            .Include(s => s.Games)
            .Include(x => x.Course)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}