using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class QuizRepository:BaseRepository<Quiz>, IQuizRepository
{
    public QuizRepository(AppDbContext context, ILogger<BaseRepository<Quiz>> logger) : base(context, logger)
    {
    }
}