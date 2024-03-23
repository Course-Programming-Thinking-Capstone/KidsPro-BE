using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IAccountRepository : IBaseRepository<Account>
{
    Task<Account?> ExistByEmailAsync(string email);

    Task<Account?> LoginByEmailAsync(string email);

    Task<Account?> GetStudentAccountById(int accountId);
    Task<Account?> GetParentAccountById(int accountId);
    Task<Account?> GetAdminAccountById(int accountId);
    Task<Account?> GetTeacherAccountById(int accountId);
    Task<Account?> GetStaffAccountById(int accountId);
}