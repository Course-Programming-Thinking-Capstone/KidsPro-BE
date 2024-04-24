using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IClassRepository:IBaseRepository<Class>
{
    Task<bool> ExistByClassCodeAsync(string code);
    Task<List<Class>> GetClassByRoleAsync(int id, string role);

    Task<Class?> GetClassByCodeAsync(string code);
}