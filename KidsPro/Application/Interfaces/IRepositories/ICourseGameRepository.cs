using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ICourseGameRepository:IBaseRepository<CourseGame>
{
    Task<List<CourseGame>> GetAvailableCourseGameAsync();
}