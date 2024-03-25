using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IClassRepository:IBaseRepository<Class>
{
    Task<bool> ExistByClassCode(string code);
}