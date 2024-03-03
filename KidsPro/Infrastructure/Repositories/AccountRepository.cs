using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    public AccountRepository(AppDbContext context, ILogger<BaseRepository<Account>> logger) : base(context, logger)
    {
    }

    public async Task<bool> ExistByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.Email == email)
            .ContinueWith(t => t.Result != null);
    }

    public async Task<Account?> LoginByEmailAsync(string email)
    {
        return await _dbSet.Include(a => a.Role).FirstOrDefaultAsync(a =>
            a.Email == email && !a.IsDelete && a.Status == UserStatus.Active);
    }

    public async Task<Account?> GetStudentAccountById(int accountId)
    {
        return await _dbSet.Include(a => a.Role)
            .Include(a => a.Student)
            .FirstOrDefaultAsync(a => a.Id == accountId && !a.IsDelete && a.Status == UserStatus.Active);
    }

    public async Task<Account?> GetParentAccountById(int accountId)
    {
        return await _dbSet.Include(a => a.Role)
            .Include(a => a.Parent)
            .FirstOrDefaultAsync(a => a.Id == accountId && !a.IsDelete && a.Status == UserStatus.Active);
    }

    public async Task<Account?> GetAdminAccountById(int accountId)
    {
        return await _dbSet.Include(a => a.Role)
            .Include(a => a.Admin)
            .FirstOrDefaultAsync(a => a.Id == accountId && !a.IsDelete && a.Status == UserStatus.Active);
    }

    public async Task<Account?> GetTeacherAccountById(int accountId)
    {
        return await _dbSet.Include(a => a.Role)
            .Include(a => a.Teacher)
            .FirstOrDefaultAsync(a => a.Id == accountId && !a.IsDelete && a.Status == UserStatus.Active);
    }

    public async Task<Account?> GetStaffAccountById(int accountId)
    {
        return await _dbSet.Include(a => a.Role)
            .Include(a => a.Staff)
            .FirstOrDefaultAsync(a => a.Id == accountId && !a.IsDelete && a.Status == UserStatus.Active);
    }
}