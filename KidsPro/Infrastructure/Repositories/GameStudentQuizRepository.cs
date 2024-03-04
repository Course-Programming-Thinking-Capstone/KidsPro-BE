using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GameStudentQuizRepository : BaseRepository<GameStudentQuiz>, IGameStudentQuizRepository
{
    public GameStudentQuizRepository(AppDbContext context, ILogger<BaseRepository<GameStudentQuiz>> logger) : base(
        context, logger)
    {
    }
}