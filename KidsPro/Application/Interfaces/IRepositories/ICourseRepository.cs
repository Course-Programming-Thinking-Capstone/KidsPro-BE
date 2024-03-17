using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ICourseRepository:IBaseRepository<Course>
{
    Task<Course?> GetCoursePayment(int id, bool disableTracking = false);
    Task<Course?> CheckCourseExist(int id);
}