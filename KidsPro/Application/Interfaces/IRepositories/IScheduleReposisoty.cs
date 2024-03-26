using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IScheduleReposisoty:IBaseRepository<ClassSchedule>
{
    Task<List<ClassSchedule>> GetScheduleByClassIdAsync(int classId);
}