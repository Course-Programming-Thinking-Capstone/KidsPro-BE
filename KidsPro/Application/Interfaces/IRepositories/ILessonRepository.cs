using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ILessonRepository:IBaseRepository<Lesson>
{

    Task<Lesson?> GetCommonLessonDetailByIdAsync(int lessonId);

    Task<Lesson?> GetTeacherLessonDetailByIdAsync(int lessonId, int teacherId);

    Task<Lesson?> GetStudentLessonDetailByIdAsync(int lessonId, int studentId);
}