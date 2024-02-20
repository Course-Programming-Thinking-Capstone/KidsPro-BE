﻿using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameVersionRepository : BaseRepository<GameVersion>
{
    public GameVersionRepository(AppDbContext context, ILogger<BaseRepository<GameVersion>> logger) : base(context,
        logger)
    {
    }
}