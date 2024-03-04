using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameLevelModifierRepository:BaseRepository<GameLevelModifier>, IGameLevelModifierRepository
{
    public GameLevelModifierRepository(AppDbContext context, ILogger<BaseRepository<GameLevelModifier>> logger) : base(context, logger)
    {
    }
}