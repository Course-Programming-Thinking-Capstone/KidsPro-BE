using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameUserProfileRepository:BaseRepository<GameUserProfile>, IGameUserProfileRepository
{
    public GameUserProfileRepository(AppDbContext context, ILogger<BaseRepository<GameUserProfile>> logger) : base(context, logger)
    {
    }
}