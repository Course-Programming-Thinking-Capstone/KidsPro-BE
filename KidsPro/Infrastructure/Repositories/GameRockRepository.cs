using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameRockRepository : BaseRepository<GameRockPosition>, IGameRockRepository
{
    public GameRockRepository(AppDbContext context, ILogger<BaseRepository<GameRockPosition>> logger) : base(context, logger)
    {
    }
}