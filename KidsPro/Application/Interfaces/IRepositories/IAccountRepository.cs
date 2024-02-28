using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IAccountRepository:IBaseRepository<Account>
{
    Task<bool> ExistByEmailAsync(string email);
}