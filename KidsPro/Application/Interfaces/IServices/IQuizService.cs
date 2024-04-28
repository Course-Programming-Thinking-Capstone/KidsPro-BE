using Application.Dtos.Request.Course.Quiz;
using Application.Dtos.Response.Course.Quiz;

namespace Application.Interfaces.IServices;

public interface IQuizService
{
    Task<QuizSubmitResponse> StudentSubmitQuizAsync(QuizSubmitRequest dto);
}