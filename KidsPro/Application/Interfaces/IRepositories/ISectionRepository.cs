using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.IRepositories;

public interface ISectionRepository:IBaseRepository<Section>
{
    Task<bool> ExistByOrderAsync(int courseId, int order);

    Task<Section?> GetStudySectionByIdAsync(int id, List<CourseStatus> courseStatuses);

    Task<Section?> GetTeacherSectionDetailByIdAsync(int sectionId, int teacherId);

    Task<Section?> GetStudentSectionDetailByIdAsync(int sectionId, int studentId);
}