using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.IRepositories;

public interface IScheduleReposisoty:IBaseRepository<ClassSchedule>
{
    Task<List<ClassSchedule>> GetScheduleByClassIdAsync(int classId, ScheduleStatus status);
}