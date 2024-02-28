using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class ParentRepository : BaseRepository<Parent>, IParentRepository
{
    public ParentRepository(AppDbContext context, ILogger<BaseRepository<Parent>> logger) : base(context, logger)
    {
    }

    public async Task<Parent?> LoginByPhoneNumberAsync(string phoneNumber)
    {
        return await _dbSet.Include(p => p.Account)
            .ThenInclude(a => a.Role)
            .FirstOrDefaultAsync(p =>
                p.PhoneNumber == phoneNumber && !p.Account.IsDelete && p.Account.Status == UserStatus.Active);
    }

    public async Task<bool> ExistByPhoneNumberAsync(string phoneNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber)
            .ContinueWith(t => t.Result != null);
    }
}