using Application.Dtos.Request.Course.Quiz;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities;

namespace Application.Services;

public class QuizService : IQuizService
{
    private IUnitOfWork _unit;
    private IAccountService _account;

    public QuizService(IUnitOfWork unit, IAccountService account)
    {
        _unit = unit;
        _account = account;
    }

    public async Task StudentSubmitQuizAsync(QuizSubmitRequest dto)
    {
        var account = await _account.GetCurrentAccountInformationAsync();

        var student = await _unit.StudentRepository.GetByIdAsync(account.IdSubRole)
                      ?? throw new NotFoundException($"StudentId {account.IdSubRole} not found");

        dto.StudentId = student.Id;
        
        //Check xem student đã làm quiz này chưa
        var studentQuizExist = await _unit.StudentQuizRepository.GetStudentQuizByFk(dto.StudentId, dto.QuizId);
          
        //Nếu đã làm rồi thì cộng dồn số turn
        if (studentQuizExist != null)
        {
            dto.Attempt += studentQuizExist.Attempt;

            if (dto.Attempt >= 4)
                throw new ConflictException(
                    "Student have run out of turns to do the quiz, the total number of turns is 3");
        }
        var studentQuiz = QuizMapper.StudentQuizRequestToStudentQuiz(dto);
        
        //Add data to studentOption table
        var studentOptions =dto.QuizResults.Select(x => new StudentOption()
        {
            StudentQuiz = studentQuiz,
            QuestionId = x.QuestionId,
            OptionId = x.OptionId
        }).ToList();

        if (studentQuizExist == null)
            await _unit.StudentOptionRepository.AddRangeAsync(studentOptions);
        else
             _unit.StudentOptionRepository.UpdateRange(studentOptions);

        await _unit.SaveChangeAsync();
        
        
    }
}