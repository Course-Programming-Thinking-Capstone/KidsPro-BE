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

    public async Task<Course?> GetCourseDetailByIdAndStatusAsync(int courseId, List<CourseStatus> statuses)
    {
        return await _dbSet
            .Where(course =>
                course.Id == courseId &&
                !course.IsDelete &&
                statuses.Contains(course.Status))
            .Select(course => new Course()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                PictureUrl = course.PictureUrl,
                IsFree = course.IsFree,
                Sections = course.Sections.OrderBy(lesson => lesson.Order).Select(s => new Section()
                {
                    Id = s.Id,
                    Name = s.Name,
                    SectionTime = s.SectionTime,
                    Order = s.Order,
                    Lessons = s.Lessons.Select(lesson => new Lesson()
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
                }).ToList(),
            }).FirstOrDefaultAsync();
    }

    public async Task<Course?> GetTeacherCourseDetailByIdAsync(int courseId, int teacherId)
    {
        return await _dbSet
            .Include(c => c.Classes)
            .Where(course =>
                course.Id == courseId &&
                !course.IsDelete &&
                (course.Status == CourseStatus.Active || course.ModifiedById == teacherId ||
                 course.Classes.Any(c => c.Teacher != null && c.Teacher.AccountId == teacherId)))
            .Select(course => new Course()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                PictureUrl = course.PictureUrl,
                IsFree = course.IsFree,
                Sections = course.Sections.OrderBy(s => s.Order).Select(s => new Section()
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
                }).ToList(),
            }).FirstOrDefaultAsync();
    }

    public async Task<Course?> GetStudentCourseDetailByIdAsync(int courseId, int studentId)
    {
        return await _dbSet
            .Where(course =>
                course.Id == courseId &&
                !course.IsDelete
                && course.Classes.Any(c => c.Students.Any(s => s.AccountId == studentId))
            )
            .Select(course => new Course()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                PictureUrl = course.PictureUrl,
                IsFree = course.IsFree,
                Sections = course.Sections.OrderBy(s => s.Order).Select(s => new Section()
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
                        StudentLessons = lesson.StudentLessons != null
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
                }).ToList(),
            })
            .FirstOrDefaultAsync();
    }
}