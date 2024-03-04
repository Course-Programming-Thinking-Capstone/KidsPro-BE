using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameRepository:BaseRepository<Game>, IGameRepository
{
    public GameRepository(AppDbContext context, ILogger<BaseRepository<Game>> logger) : base(context, logger)
    {
    }
}