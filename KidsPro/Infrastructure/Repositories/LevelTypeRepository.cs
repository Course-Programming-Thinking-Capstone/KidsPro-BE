using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class LevelTypeRepository : BaseRepository<LevelType>, ILevelTypeRepository
{
    public LevelTypeRepository(AppDbContext context, ILogger<BaseRepository<LevelType>> logger) : base(context, logger)
    {
    }

    public async Task ForceAddRangeAsync(IEnumerable<LevelType> entities)
    {
        try
        {
            // Enable IDENTITY_INSERT
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.LevelTypes ON");
            // Add entities
            await AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            // Disable IDENTITY_INSERT
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.LevelTypes OFF");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while inserting entities with IDENTITY_INSERT enabled.");
            throw;
        }
    }
}