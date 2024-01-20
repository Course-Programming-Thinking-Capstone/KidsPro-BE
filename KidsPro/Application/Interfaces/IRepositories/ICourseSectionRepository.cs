using System.Linq.Expressions;
using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ICourseSectionRepository : IBaseRepository<CourseSection>
{
    Task DeleteByCourseIdAsync(int courseId);
    Task IncreaseSectionOrderAsync(int courseId, int order);
    Task DecreaseSectionOrderAsync(int courseId, int order);

    Task<CourseSection?> GetFirstByFilterAsync(
        Expression<Func<CourseSection, bool>>? filter,
        Func<IQueryable<CourseSection>, IOrderedQueryable<CourseSection>> orderBy,
        string? includeProperties = null,
        bool disableTracking = false);
}