using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameLevelRepository: BaseRepository<GameLevel>
{
    public GameLevelRepository(AppDbContext context, ILogger<BaseRepository<GameLevel>> logger) : base(context, logger)
    {
    }
}