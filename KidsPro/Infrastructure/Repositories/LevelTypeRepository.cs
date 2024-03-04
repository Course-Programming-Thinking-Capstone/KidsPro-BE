using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class LevelTypeRepository:BaseRepository<LevelType>, IBaseRepository<LevelType>
{
    public LevelTypeRepository(AppDbContext context, ILogger<BaseRepository<LevelType>> logger) : base(context, logger)
    {
    }
}