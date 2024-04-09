using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ICourseRepository:IBaseRepository<Course>
{
    Task<Course?> GetCoursePayment(int courseId,int classId, bool disableTracking = false);
    Task<Course?> CheckCourseExist(int id);
}