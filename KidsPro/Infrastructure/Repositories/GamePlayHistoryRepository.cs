using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GamePlayHistoryRepository:BaseRepository<GamePlayHistory>, IGamePlayHistoryRepository
{
    public GamePlayHistoryRepository(AppDbContext context, ILogger<BaseRepository<GamePlayHistory>> logger) : base(context, logger)
    {
    }
}