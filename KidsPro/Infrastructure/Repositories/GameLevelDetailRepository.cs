using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameLevelDetailRepository:BaseRepository<GameLevelDetail>, IGameLevelDetailRepository
{
    public GameLevelDetailRepository(AppDbContext context, ILogger<BaseRepository<GameLevelDetail>> logger) : base(context, logger)
    {
    }
}