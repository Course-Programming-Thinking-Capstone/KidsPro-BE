using Application.Dtos.Response.Course;
using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.IRepositories;

public interface ISectionRepository:IBaseRepository<Section>
{
    Task<bool> ExistByOrderAsync(int courseId, int order);

    Task<Section?> GetStudySectionById(int id, List<CourseStatus> courseStatuses);
}