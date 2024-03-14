using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Lesson;
using Application.Dtos.Request.Course.Quiz;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Request.Course.Update.Quiz;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.Lesson;
using Application.Dtos.Response.Course.Quiz;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Options;

namespace Application.Mappers;

public static class CourseMapper
{
    // public static Course CreateCourseDtoToEntity(CreateCourseDto dto)
    //     => new Course()
    //     {
    //         Name = dto.Name,
    //         Description = dto.Description
    //     };

    public static SectionDto SectionToSectionDto(Section entity)
        => new SectionDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Order = entity.Order,
            CourseId = entity.CourseId,
            Lessons = entity.Lessons.Select(LessonToLessonDto).ToList(),
            Quizzes = entity.Quizzes.Select(QuizToQuizDto).ToList()
        };

    public static List<SectionDto> SectionToSectionDto(IEnumerable<Section> entities)
        => entities.Select(SectionToSectionDto).ToList();

    public static CourseDto CourseToCourseDto(Course entity)
        => new CourseDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            CreatedDate = DateUtils.FormatDateTimeToDatetimeV1(entity.CreatedDate),
            PictureUrl = entity.PictureUrl,
            Status = entity.Status.ToString(),
            DiscountPrice = entity.DiscountPrice,
            ModifiedDate = DateUtils.FormatDateTimeToDatetimeV1(entity.ModifiedDate),
            TotalLesson = entity.TotalLesson,
            CreatedById = entity.CreatedById,
            CreatedByName = entity.CreatedBy.FullName,
            EndSaleDate = DateUtils.FormatDateTimeToDatetimeV1(entity.EndSaleDate),
            ModifiedById = entity.ModifiedById,
            ModifiedByName = entity.ModifiedBy.FullName,
            StartSaleDate = DateUtils.FormatDateTimeToDatetimeV1(entity.StartSaleDate),
            IsFree = entity.IsFree,
            Sections = entity.Sections.Select(SectionToSectionDto).ToList()
        };

    public static SectionComponentNumberDto EntityToSectionComponentNumberDto(SectionComponentNumber entity)
        => new SectionComponentNumberDto()
        {
            Name = entity.SectionComponentType.ToString(),
            MaxNumber = entity.MaxNumber
        };

    public static List<SectionComponentNumberDto> EntityToSectionComponentNumberDto(
        IEnumerable<SectionComponentNumber> entities)
        => entities.Select(EntityToSectionComponentNumberDto).ToList();


    public static Lesson CreateLessonDtoToLesson(CreateLessonDto dto)
        => new Lesson()
        {
            Name = dto.Name,
            Order = dto.Order,
            IsFree = dto.IsFree,
            Type = dto.Type,
            Content = dto.Content,
            Duration = dto.Duration,
            ResourceUrl = dto.ResourceUrl
        };


    public static LessonDto LessonToLessonDto(Lesson entity)
    {
        return entity.Type switch
        {
            LessonType.Video => new VideoDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Order = entity.Order,
                IsFree = entity.IsFree,
                Type = entity.Type.ToString(),
                Duration = entity.Duration,
                IsDelete = entity.IsDelete,
                ResourceUrl = entity.ResourceUrl,
                SectionId = entity.SectionId
            },
            LessonType.Document => new DocumentDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Order = entity.Order,
                IsFree = entity.IsFree,
                Type = entity.Type.ToString(),
                Duration = entity.Duration,
                IsDelete = entity.IsDelete,
                Content = entity.Content,
                SectionId = entity.SectionId
            },
            _ => throw new UnsupportedException("Unsupported lesson type.")
        };
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
            MinScore = dto.MinScore ?? 0,
            Title = dto.Title,
            Description = dto.Description,
            Duration = dto.Duration,
            NumberOfAttempt = dto.NumberOfAttempt,
            IsOrderRandom = dto.IsOrderRandom ?? false
        };


    public static List<LessonDto> LessonToLessonDto(IEnumerable<Lesson> entities)
        => entities.Select(LessonToLessonDto).ToList();

    public static OptionDto OptionToOptionDto(Option entity)
        => new OptionDto()
        {
            Order = entity.Order,
            Content = entity.Content,
            AnswerExplain = entity.AnswerExplain,
            IsCorrect = entity.IsCorrect
        };

    public static QuestionDto QuestionToQuestionDto(Question entity)
        => new QuestionDto()
        {
            Order = entity.Order,
            Title = entity.Title,
            Score = entity.Score,
            Options = entity.Options.Select(OptionToOptionDto).ToList()
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
            MinScore = entity.MinScore,
            TotalQuestion = entity.TotalQuestion,
            TotalScore = entity.TotalScore,
            NumberOfQuestion = entity.NumberOfQuestion,
            CreatedDate = DateUtils.FormatDateTimeToDatetimeV1(entity.CreatedDate),
            CreatedById = entity.CreatedById,
            CreatedByName = entity.CreatedBy.FullName,
            Questions = entity.Questions.Select(QuestionToQuestionDto).ToList()
        };

    public static Lesson UpdateLessonDtoToLesson(Dtos.Request.Course.Update.Lesson.UpdateLessonDto dto)
        => new Lesson()
        {
            Name = dto.Name ?? throw new BadRequestException("Lesson name is required!"),
            Duration = dto.Duration,
            Content = dto.Content,
            ResourceUrl = dto.ResourceUrl,
            Type = dto.Type ?? throw new BadRequestException("Lesson type is required!"),
            IsFree = dto.IsFree ?? false
        };

    public static void UpdateLessonDtoToLesson(Dtos.Request.Course.Update.Lesson.UpdateLessonDto dto, ref Lesson entity)
    {
        if (!dto.Id.HasValue)
            throw new BadRequestException("Lesson Id can not be null.");

        if (!string.IsNullOrEmpty(dto.Name))
        {
            entity.Name = dto.Name;
        }

        if (dto.Duration.HasValue)
        {
            entity.Duration = dto.Duration;
        }

        if (!string.IsNullOrEmpty(dto.Content))
        {
            entity.Content = dto.Content;
        }

        if (!string.IsNullOrEmpty(dto.ResourceUrl))
        {
            entity.ResourceUrl = dto.ResourceUrl;
        }

        if (dto.IsFree.HasValue)
        {
            entity.IsFree = dto.IsFree.Value;
        }
    }

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
            MinScore = dto.MinScore ?? 0,
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
        if (dto.MinScore.HasValue)
            entity.MinScore = dto.MinScore.Value;
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

    public static FilterCourseDto CourseToFilterCourseDto(Course entity)
        => new FilterCourseDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            IsFree = entity.IsFree,
            PictureUrl = entity.PictureUrl,
            Status = entity.Status.ToString()
        };

    public static PagingResponse<FilterCourseDto> CourseToFilterCourseDto(PagingResponse<Course> entities)
        => new PagingResponse<FilterCourseDto>()
        {
            TotalPages = entities.TotalPages,
            TotalRecords = entities.TotalRecords,
            Results = entities.Results.Select(CourseToFilterCourseDto).ToList()
        };

    public static void UpdateCourseDtoToEntity(UpdateCourseDto dto, ref Course entity)
    {
        if (!string.IsNullOrEmpty(dto.Name))
        {
            entity.Name = dto.Name;
        }

        if (!string.IsNullOrEmpty(dto.Description))
        {
            entity.Description = dto.Description;
        }

        if (dto.IsFree.HasValue)
        {
            entity.IsFree = dto.IsFree.Value;
        }
    }

    public static void UpdateSectionDtoToSection(UpdateSectionDto dto, ref Section entity)
    {
        if (!string.IsNullOrEmpty(dto.Name))
        {
            entity.Name = dto.Name;
        }
    }


    public static void UpdateLessonDtoToLesson(UpdateLessonDto dto, ref Lesson entity)
    {
        if (!string.IsNullOrEmpty(dto.Name))
        {
            entity.Name = dto.Name;
        }

        if (dto.Duration.HasValue)
        {
            entity.Duration = dto.Duration.Value;
        }

        if (dto.IsFree.HasValue)
        {
            entity.IsFree = dto.IsFree.Value;
        }

        if (dto is UpdateVideoDto updateVideoDto)
        {
            if (!string.IsNullOrEmpty(updateVideoDto.ResourceUrl))
                entity.ResourceUrl = updateVideoDto.ResourceUrl;
        }
        else if (dto is UpdateDocumentDto updateDocumentDto)
        {
            if (!string.IsNullOrEmpty(updateDocumentDto.Content))
            {
                entity.Content = updateDocumentDto.Content;
            }
        }
    }
}