using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IRoleRepository:IBaseRepository<Role>
{
    Task<Role?> GetByNameAsync(string roleName);
}