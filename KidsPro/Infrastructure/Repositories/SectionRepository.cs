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

    public async Task<Section?> GetStudySectionByIdAsync(int id, List<CourseStatus> courseStatuses)
    {
        return await _dbSet.Where(s => s.Id == id && !s.Course.IsDelete && courseStatuses.Contains(s.Course.Status))
            .Select(s => new Section()
            {
                Id = s.Id,
                Name = s.Name,
                SectionTime = s.SectionTime,
                Order = s.Order,
                Lessons = s.Lessons.OrderBy(lesson => lesson.Order).Select(lesson => new Lesson()
                {
                    Id = lesson.Id,
                    Name = lesson.Name,
                    Duration = lesson.Duration,
                    Type = lesson.Type,
                    IsFree = lesson.IsFree
                }).ToList(),
                Quizzes = s.Quizzes.OrderBy(q => q.Order).Select(quiz => new Quiz()
                {
                    Id = quiz.Id,
                    TotalQuestion = quiz.TotalQuestion,
                    Duration = quiz.Duration,
                    Title = quiz.Title
                }).ToList()
            }).FirstOrDefaultAsync();
    }

    public async Task<Section?> GetTeacherSectionDetailByIdAsync(int sectionId, int teacherId)
    {
        return await _dbSet.Where(s =>
                s.Id == sectionId && !s.Course.IsDelete &&
                (s.Course.Status == CourseStatus.Active ||
                 s.Course.ModifiedById == teacherId ||
                 s.Course.Classes.Any(c => c.Teacher != null && c.Teacher.AccountId == teacherId)))
            .Select(s => new Section()
            {
                Id = s.Id,
                Name = s.Name,
                SectionTime = s.SectionTime,
                Order = s.Order,
                Lessons = s.Lessons.OrderBy(lesson => lesson.Order).Select(lesson => new Lesson()
                {
                    Id = lesson.Id,
                    Name = lesson.Name,
                    Duration = lesson.Duration,
                    Type = lesson.Type,
                    IsFree = lesson.IsFree
                }).ToList(),
                Quizzes = s.Quizzes.OrderBy(q => q.Order).Select(quiz => new Quiz()
                {
                    Id = quiz.Id,
                    TotalQuestion = quiz.TotalQuestion,
                    Duration = quiz.Duration,
                    Title = quiz.Title
                }).ToList()
            }).FirstOrDefaultAsync();
    }

    public async Task<Section?> GetStudentSectionDetailByIdAsync(int sectionId, int studentId)
    {
        return await _dbSet.Where(s =>
                s.Id == sectionId && !s.Course.IsDelete &&
                s.Course.Classes.Any(c => c.Students.Any(student => student.AccountId == studentId))
            )
            .Select(s => new Section()
            {
                Id = s.Id,
                Name = s.Name,
                SectionTime = s.SectionTime,
                Order = s.Order,
                Lessons = s.Lessons.OrderBy(lesson => lesson.Order).Select(lesson => new Lesson()
                {
                    Id = lesson.Id,
                    Name = lesson.Name,
                    Duration = lesson.Duration,
                    Type = lesson.Type,
                    IsFree = lesson.IsFree,
                    StudentLessons =
                        lesson.StudentLessons != null
                            ? lesson.StudentLessons
                                .Where(sl => sl.Student.AccountId == studentId && sl.LessonId == lesson.Id).ToList()
                            : null,
                }).ToList(),
                Quizzes = s.Quizzes.OrderBy(q => q.Order).Select(quiz => new Quiz()
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
            .Include(x => x.Course)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}