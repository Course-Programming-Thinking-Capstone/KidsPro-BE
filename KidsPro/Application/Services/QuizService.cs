using Application.Dtos.Request.Course.Quiz;
using Application.Dtos.Response.Course.Quiz;
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

    public async Task<QuizSubmitResponse> StudentSubmitQuizAsync(QuizSubmitRequest dto)
    {
        var account = await _account.GetCurrentAccountInformationAsync();

        var student = await _unit.StudentRepository.GetByIdAsync(account.IdSubRole)
                      ?? throw new NotFoundException($"StudentId {account.IdSubRole} not found");

        dto.StudentId = student.Id;

        //Check xem student đã làm quiz này chưa
        var studentQuizExist = await _unit.StudentQuizRepository.GetStudentQuizByFk(dto.StudentId, dto.QuizId);

        if (studentQuizExist?.Attempt >= 3)
            throw new ConflictException(
                "Student have run out of turns to do the quiz, the total number of turns is 3");

        var studentQuiz = QuizMapper.QuizSubmitRequestToStudentQuiz(dto);

        if (studentQuizExist == null)
        {
            //Add data to studentOption table
            var studentOptions = dto.QuizResults.Select(x => new StudentOption()
            {
                StudentQuiz = studentQuiz,
                QuestionId = x.QuestionId,
                OptionId = x.OptionId
            }).ToList();
            await _unit.StudentOptionRepository.AddRangeAsync(studentOptions);
        }
        else
        {
            //Update data to studentOption table
            foreach (var (sa, qr) in studentQuizExist.StudentAnswers.Zip(dto.QuizResults))
            {
                sa.OptionId = qr.OptionId;
            }

            _unit.StudentOptionRepository.UpdateRange(studentQuizExist.StudentAnswers);
        }

        await _unit.SaveChangeAsync();

        studentQuiz = await CalculateQuizScore(studentQuizExist?.Id ?? studentQuiz.Id);

        return QuizMapper.StudentQuizToQuizSubmitResponse(studentQuiz, student);
    }

    private async Task<StudentQuiz> CalculateQuizScore(int studentQuizId)
    {
        var studentQuiz = await _unit.StudentQuizRepository.GetByIdAsync(studentQuizId)
                          ?? throw new NotFoundException(
                              $"StudentQuizID {studentQuizId} not found, entity has not been saved");

        //int ratio = 100;
        int passRatio = studentQuiz.Quiz.PassCondition!.PassRatio;
        float questionRatio = (float)100 / studentQuiz.Quiz.TotalQuestion;

        //Check the number correct answer of student
        var numberCorrect = studentQuiz.Quiz.Questions
            .Count(q => q.Options
                .Any(o => studentQuiz.StudentAnswers
                    .Any(s => o.Id == s.OptionId) && o.IsCorrect));
        //Score
        float correctAnswerRatio = questionRatio * numberCorrect;

        if (correctAnswerRatio >= passRatio)
            studentQuiz.IsPass = true;
        else
            studentQuiz.IsPass = false;

        //Calculate score, convert to percent
        studentQuiz.Score = (decimal)correctAnswerRatio / 10;
        studentQuiz.Attempt += 1;

        _unit.StudentQuizRepository.Update(studentQuiz);
        await _unit.SaveChangeAsync();
        return studentQuiz;
    }
}