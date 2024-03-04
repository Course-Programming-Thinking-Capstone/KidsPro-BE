﻿using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class LevelTypeRepository : BaseRepository<LevelType>, ILevelTypeRepository
{
    public LevelTypeRepository(AppDbContext context, ILogger<BaseRepository<LevelType>> logger) : base(context, logger)
    {
    }
}