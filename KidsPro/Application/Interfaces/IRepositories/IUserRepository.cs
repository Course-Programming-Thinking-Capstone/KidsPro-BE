using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IUserRepository:IBaseRepository<User>
{
    Task<User?> GetUserByAttribute(string at1, string? at2,int type);
    Task<List<User>> GetAllUsersByRole(int role);
}