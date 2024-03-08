using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameQuizRoomRepository:BaseRepository<MiniGame>, IGameQuizRoomRepository
{
    public GameQuizRoomRepository(AppDbContext context, ILogger<BaseRepository<MiniGame>> logger) : base(context, logger)
    {
    }
}