using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameLeverModifierRepository  : BaseRepository<GameLevelModifier>
{
    public GameLeverModifierRepository(AppDbContext context, ILogger<BaseRepository<GameLevelModifier>> logger) : base(context, logger)
    {
    }
}