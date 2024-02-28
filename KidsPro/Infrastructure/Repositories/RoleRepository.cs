using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class RoleRepository:BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context, ILogger<BaseRepository<Role>> logger) : base(context, logger)
    {
    }

    public async Task<Role?> GetByNameAsync(string roleName)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Name == roleName);
    }
}