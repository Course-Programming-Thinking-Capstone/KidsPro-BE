using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IStudentQuizRepository:IBaseRepository<StudentQuiz>
{
    Task<StudentQuiz?> GetStudentQuizByFk(int studentId, int quizId);
}