using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class LessonRepository : BaseRepository<Lesson>, ILessonRepository
{
    public LessonRepository(AppDbContext context, ILogger<BaseRepository<Lesson>> logger) : base(context, logger)
    {
    }

    public async Task<Lesson?> GetCommonLessonDetailByIdAsync(int lessonId)
    {
        return await _dbSet.Where(l =>
                l.IsFree || (l.Section.Course.Status == CourseStatus.Active && l.Section.Course.IsFree))
            .Select(lesson => new Lesson()
            {
                Id = lesson.Id,
                Name = lesson.Name,
                Duration = lesson.Duration,
                IsFree = lesson.IsFree,
                Content = lesson.Content,
                ResourceUrl = lesson.ResourceUrl,
                Type = lesson.Type,
                SectionId = lesson.SectionId
            }).FirstOrDefaultAsync();
    }

    public async Task<Lesson?> GetTeacherLessonDetailByIdAsync(int lessonId, int teacherId)
    {
        return await _dbSet
            .Where(lesson => lesson.Id == lessonId
                             && !lesson.Section.Course.IsDelete
                             && (lesson.IsFree ||
                                 (lesson.Section.Course.Status == CourseStatus.Active &&
                                  lesson.Section.Course.IsFree)
                                 || lesson.Section.Course.ModifiedById == teacherId
                                 || lesson.Section.Course.Classes.Any(c =>
                                     c.Teacher != null && c.Teacher.AccountId == teacherId))
            )
            .Select(lesson => new Lesson()
            {
                Id = lesson.Id,
                Name = lesson.Name,
                Duration = lesson.Duration,
                IsFree = lesson.IsFree,
                Content = lesson.Content,
                ResourceUrl = lesson.ResourceUrl,
                Type = lesson.Type,
                SectionId = lesson.SectionId,
            }).FirstOrDefaultAsync();
    }

    public async Task<Lesson?> GetStudentLessonDetailByIdAsync(int lessonId, int studentId)
    {
        return await _dbSet
            .Where(
                lesson => lesson.Id == lessonId
                          && !lesson.Section.Course.IsDelete
                          && (lesson.IsFree
                              || (lesson.Section.Course.Status == CourseStatus.Active
                                  && lesson.Section.Course.IsFree)
                              || lesson.Section.Course.Classes.Any(c => c.Students.Any(s => s.AccountId == studentId)))
            )
            .Select(lesson => new Lesson()
            {
                Id = lesson.Id,
                Name = lesson.Name,
                Duration = lesson.Duration,
                IsFree = lesson.IsFree,
                Content = lesson.Content,
                ResourceUrl = lesson.ResourceUrl,
                Type = lesson.Type,
                SectionId = lesson.SectionId,
                StudentLessons = lesson.StudentLessons != null
                    ? lesson.StudentLessons
                        .Where(sl => sl.Student.AccountId == studentId && sl.LessonId == lesson.Id).ToList()
                    : null,
            }).FirstOrDefaultAsync();
    }

    public override async Task<Lesson?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Lesson> query = _dbSet;

        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await query.Where(l => l.Id == id).FirstOrDefaultAsync();
    }
}