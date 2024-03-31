using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ITeacherRepository:IBaseRepository<Teacher>
{
    Task<List<Teacher>> GetTeacherSchedules();
    Task<Teacher> GetTeacherSchedulesById(int teacherId);
}