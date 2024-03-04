using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameQuizRoomRepository:BaseRepository<GameQuizRoom>, IGameQuizRoomRepository
{
    public GameQuizRoomRepository(AppDbContext context, ILogger<BaseRepository<GameQuizRoom>> logger) : base(context, logger)
    {
    }
}