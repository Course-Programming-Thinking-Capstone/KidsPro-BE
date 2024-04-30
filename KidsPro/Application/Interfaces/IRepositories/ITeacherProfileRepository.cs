using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ITeacherProfileRepository:IBaseRepository<TeacherProfile>
{
    Task<List<TeacherProfile>> GetTeacherProfiles(int teacherId);
}