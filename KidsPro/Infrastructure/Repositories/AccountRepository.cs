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

    public async Task<Account?> ExistByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<Account?> LoginByEmailAsync(string email)
    {
        return await _dbSet.Include(a => a.Role).FirstOrDefaultAsync(a =>
            a.Email == email && !a.IsDelete && a.Status != UserStatus.Inactive);
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
            .Include(a => a.Teacher).ThenInclude(x=>x.TeacherProfiles)
            .FirstOrDefaultAsync(a => a.Id == accountId && !a.IsDelete && a.Status == UserStatus.Active);
    }

    public async Task<Account?> GetStaffAccountById(int accountId)
    {
        return await _dbSet.Include(a => a.Role)
            .Include(a => a.Staff)
            .FirstOrDefaultAsync(a => a.Id == accountId && !a.IsDelete && a.Status == UserStatus.Active);
    }

    public override async Task<Account?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Account> query = _dbSet;

        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await query.Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDelete && a.Status != UserStatus.Inactive);
    }
    
    public  async Task<Account?> AdminGetAccountById(int id)
    {
        IQueryable<Account> query = _dbSet;

        return await query.Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public override Task<List<Account>> GetAllAsync()
    {
        IQueryable<Account> query = _dbSet;

        return query.Include(a => a.Role)
            .ToListAsync();
    }
}