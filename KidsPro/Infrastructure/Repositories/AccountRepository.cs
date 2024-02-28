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
}