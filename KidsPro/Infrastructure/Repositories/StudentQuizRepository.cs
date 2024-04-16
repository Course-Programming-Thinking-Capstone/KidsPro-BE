using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class StudentQuizRepository:BaseRepository<StudentQuiz>,IStudentQuizRepository
{
    public StudentQuizRepository(AppDbContext context, ILogger<BaseRepository<StudentQuiz>> logger) : base(context, logger)
    {
    }
    
    public async Task<StudentQuiz?> GetStudentQuizByFk(int studentId, int quizId)
    {
        return await _dbSet.Include(x=>x.Quiz).Include(x=>x.Student)
            .FirstOrDefaultAsync(x => x.StudentId == studentId && x.QuizId == quizId);
    }

    public override Task<StudentQuiz?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<StudentQuiz> query = _dbSet;
        return query.Include(x => x.Quiz)
            .ThenInclude(x => x.Questions).ThenInclude(x => x.Options)
            .Include(x => x.Quiz).ThenInclude(x=>x.PassCondition)
            .Include(x => x.StudentAnswers).FirstOrDefaultAsync(x => x.Id == id);
    }
}