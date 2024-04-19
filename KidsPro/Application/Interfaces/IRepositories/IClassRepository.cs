using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IClassRepository:IBaseRepository<Class>
{
    Task<bool> ExistByClassCode(string code);
    Task<List<Class>> GetClassByRole(int id, string role);

    Task<Class?> GetClassByCode(string code);
}