using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class QuizRepository : BaseRepository<Quiz>, IQuizRepository
{
    public QuizRepository(AppDbContext context, ILogger<BaseRepository<Quiz>> logger) : base(context, logger)
    {
    }

    public override async Task<Quiz?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<Quiz> query = _dbSet;

        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await query.Where(q => q.Id == id).Include(q => q.Questions.OrderBy(question => question.Order))
            .ThenInclude(question => question.Options.OrderBy(option => option.Order))
            .FirstOrDefaultAsync();
    }
}