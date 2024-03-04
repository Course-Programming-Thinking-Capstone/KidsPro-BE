using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameItemRepository:BaseRepository<GameItem>, IGameItemRepository
{
    public GameItemRepository(AppDbContext context, ILogger<BaseRepository<GameItem>> logger) : base(context, logger)
    {
    }
}