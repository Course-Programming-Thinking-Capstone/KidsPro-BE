using Application.Dtos.Request.Course.Quiz;
using Domain.Entities;

namespace Application.Mappers;

public static class QuizMapper
{
    public static StudentQuiz StudentQuizRequestToStudentQuiz(QuizSubmitRequest dto) => new StudentQuiz()
    {
        StudentId = dto.StudentId,
        QuizId = dto.QuizId,
        Score = dto.Score,
        Attempt = dto.Attempt,
        StartTime = dto.StartTime,
        EndTime = DateTime.UtcNow,
    };
    
    
}