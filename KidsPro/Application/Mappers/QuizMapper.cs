using Application.Dtos.Request.Course.Quiz;
using Application.Dtos.Request.Course.Update.Quiz;
using Application.Dtos.Response.Course.Quiz;
using Application.ErrorHandlers;
using Application.Utils;
using Domain.Entities;

namespace Application.Mappers;

public static class QuizMapper
{
    public static StudentQuiz QuizSubmitRequestToStudentQuiz(QuizSubmitRequest dto) => new StudentQuiz()
    {
        StudentId = dto.StudentId,
        QuizId = dto.QuizId,
        Score = dto.Score,
        Attempt = 0,
        StartTime = DateTime.UtcNow.AddMinutes(-dto.QuizMinutes),
        EndTime = DateTime.UtcNow,
        IsPass = false,
        Duration = dto.Duration
    };

    public static QuizSubmitResponse StudentQuizToQuizSubmitResponse(StudentQuiz dto, Student student) =>
        new QuizSubmitResponse()
        {
            StudentId = student.Id,
            StudentName = student.Account.FullName,
            QuizSubmit = StudentQuizToQuizSubmitDto(dto)
        };

    public static QuizSubmitDto StudentQuizToQuizSubmitDto(StudentQuiz dto) => new QuizSubmitDto()
    {
        Score = Math.Round(dto.Score, 1),
        AttemptTurnNumber = 3 - dto.Attempt,
        Duration = dto.Duration,
        StartTime = DateUtils.FormatDateTimeToDatetimeV1(dto.StartTime),
        EndTime = DateUtils.FormatDateTimeToDatetimeV1(dto.EndTime),
        IsPass = dto.IsPass,
        QuestionDtos = dto.Quiz.Questions.Zip(dto.StudentAnswers, (q, a) =>
            QuestionToQuestionDto(q, a.OptionId)).ToList(),
    };

    public static QuestionDto QuestionToQuestionDto(Question entity, int studentOptionId = 0)
        => new QuestionDto()
        {
            QuestionId = entity.Id,
            Order = entity.Order,
            Title = entity.Title,
            Score = entity.Score,
            Options = entity.Options.Select(x => OptionToOptionDto(x, studentOptionId)).ToList()
        };

    public static OptionDto OptionToOptionDto(Option entity, int studentOptionId = 0)
        => new OptionDto()
        {
            OptionId = entity.Id,
            Order = entity.Order,
            Content = entity.Content,
            AnswerExplain = entity.AnswerExplain,
            IsCorrect = entity.IsCorrect,
            IsStudentChoose = entity.Id == studentOptionId
        };

    public static QuizDto QuizToQuizDto(Quiz entity)
        => new QuizDto()
        {
            Id = entity.Id,
            Order = entity.Order,
            Description = entity.Description,
            Duration = entity.Duration,
            IsOrderRandom = entity.IsOrderRandom,
            NumberOfAttempt = entity.NumberOfQuestion,
            Title = entity.Title,
            TotalQuestion = entity.TotalQuestion,
            TotalScore = entity.TotalScore,
            NumberOfQuestion = entity.NumberOfQuestion,
            CreatedDate = DateUtils.FormatDateTimeToDatetimeV1(entity.CreatedDate),
            CreatedById = entity.CreatedById,
            CreatedByName = entity.CreatedBy.FullName,
            Questions = entity.Questions.Select(QuestionToQuestionDto).ToList()
        };

    public static Option UpdateOptionDtoToOption(UpdateOptionDto dto)
        => new()
        {
            Content = dto.Content ?? throw new BadRequestException("Option content is missing!"),
            AnswerExplain = dto.AnswerExplain,
            IsCorrect = dto.IsCorrect ?? false
        };

    public static void UpdateOptionDtoToOption(UpdateOptionDto dto, ref Option entity)
    {
        if (!dto.Id.HasValue)
        {
            throw new BadRequestException("Option id is missing!");
        }

        if (!string.IsNullOrEmpty(dto.Content))
        {
            entity.Content = dto.Content;
        }

        if (!string.IsNullOrEmpty(dto.AnswerExplain))
        {
            entity.AnswerExplain = dto.AnswerExplain;
        }

        if (dto.IsCorrect.HasValue)
        {
            entity.IsCorrect = dto.IsCorrect.Value;
        }
    }

    public static Question UpdateQuestionDtoToQuestion(UpdateQuestionDto dto)
        => new Question()
        {
            Score = dto.Score ?? 1,
            Title = dto.Title ?? throw new BadRequestException($"Question title is required!")
        };

    public static void UpdateQuestionDtoToQuestion(UpdateQuestionDto dto, ref Question entity)
    {
        if (!dto.Id.HasValue)
        {
            throw new BadRequestException("Question id is missing.");
        }

        if (!string.IsNullOrEmpty(dto.Title))
            entity.Title = dto.Title;

        if (dto.Score.HasValue)
            entity.Score = dto.Score.Value;
    }

    public static Quiz UpdateQuizDtoToQuiz(UpdateQuizDto dto)
        => new Quiz()
        {
            Title = dto.Title ?? throw new BadRequestException("Quiz title is missing."),
            Description = dto.Description,
            Duration = dto.Duration,
            NumberOfAttempt = dto.NumberOfAttempt,
            NumberOfQuestion = dto.NumberOfQuestion ?? 0,
            IsOrderRandom = dto.IsOrderRandom ?? false
        };

    public static void UpdateQuizDtoToQuiz(UpdateQuizDto dto, ref Quiz entity)
    {
        if (!dto.Id.HasValue)
            throw new BadRequestException("Quiz id is missing.");
        if (!string.IsNullOrEmpty(dto.Title))
            entity.Title = dto.Title;
        if (!string.IsNullOrEmpty(dto.Description))
            entity.Description = dto.Description;
        if (dto.Duration.HasValue)
            entity.Duration = dto.Duration.Value;
        if (dto.NumberOfAttempt.HasValue)
            entity.NumberOfAttempt = dto.NumberOfAttempt;
        if (dto.NumberOfQuestion.HasValue)
            entity.NumberOfQuestion = dto.NumberOfQuestion.Value;
        if (dto.IsOrderRandom.HasValue)
            entity.IsOrderRandom = dto.IsOrderRandom.Value;
    }

    public static Option CreateOptionDtoToOption(CreateOptionDto dto)
        => new()
        {
            Content = dto.Content,
            AnswerExplain = dto.AnswerExplain,
            IsCorrect = dto.IsCorrect
        };

    public static Question CreateQuestionDtoToQuestion(CreateQuestionDto dto)
        => new()
        {
            Title = dto.Title,
            Score = dto.Score ?? 1
        };

    public static Quiz CreateQuizDtoToQuiz(CreateQuizDto dto)
        => new()
        {
            Title = dto.Title,
            Description = dto.Description,
            Duration = dto.Duration,
            NumberOfAttempt = dto.NumberOfAttempt,
            IsOrderRandom = dto.IsOrderRandom ?? false
        };
}