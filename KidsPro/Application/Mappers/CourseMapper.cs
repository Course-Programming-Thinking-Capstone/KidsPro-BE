using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Lesson;
using Application.Dtos.Request.Course.Quiz;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.CourseModeration;
using Application.Dtos.Response.Course.FilterCourse;
using Application.Dtos.Response.Course.Lesson;
using Application.Dtos.Response.Course.Quiz;
using Application.Dtos.Response.Course.Study;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using CommonCourseDto = Application.Dtos.Response.Course.CommonCourseDto;

namespace Application.Mappers;

public static class CourseMapper
{
    public const string FilterCommonCourseType = "Common";
    public const string FilterManageCourseType = "Manage";

    public static SectionDto SectionToSectionDto(Section entity)
        => new SectionDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Order = entity.Order,
            CourseId = entity.CourseId,
            Lessons = entity.Lessons.Select(LessonToLessonDto).ToList(),
            Quizzes = entity.Quizzes.Select(QuizMapper.QuizToQuizDto).ToList()
        };

    public static List<SectionDto> SectionToSectionDto(IEnumerable<Section> entities)
        => entities.Select(SectionToSectionDto).ToList();

    public static ManageCourseDto CourseToManageCourseDto(Course entity)
        => new()
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
            TotalLesson = CalculateTotal(entity, LessonType.Section),
            CreatedById = entity.CreatedById,
            CreatedByName = entity.CreatedBy.FullName,
            EndSaleDate = DateUtils.FormatDateTimeToDatetimeV1(entity.EndSaleDate),
            ModifiedById = entity.ModifiedById,
            ModifiedByName = entity.ModifiedBy?.FullName,
            ApprovedById = entity.ApprovedById,
            ApprovedByName = entity.ApprovedBy?.FullName,
            StartSaleDate = DateUtils.FormatDateTimeToDatetimeV1(entity.StartSaleDate),
            IsFree = entity.IsFree,
            Sections = entity.Sections.Select(SectionToSectionDto).ToList(),
            Classes = ClassMapper.ClassToClassesResponse(entity.Classes.ToList())
        };

    public static CommonCourseDto CourseToCommonCourseDto(Course entity)
        => new CommonCourseDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            PictureUrl = entity.PictureUrl,
            DiscountPrice = entity.DiscountPrice,
            EndSaleDate = DateUtils.FormatDateTimeToDatetimeV1(entity.EndSaleDate),
            StartSaleDate = DateUtils.FormatDateTimeToDatetimeV1(entity.StartSaleDate),
            IsFree = entity.IsFree,
            Sections = entity.Sections.Select(SectionToSectionDto).ToList(),
            Classes = ClassMapper.ClassToClassesResponse(entity.Classes.ToList()),
            TotalLesson = CalculateTotal(entity, LessonType.Section),
            TotalVideo = CalculateTotal(entity, LessonType.Video),
            TotalDocument = CalculateTotal(entity, LessonType.Document),
            TotalQuiz = CalculateTotal(entity, LessonType.Quiz)
        };

    private static int CalculateTotal(Course dto, LessonType type)
    {
        int total = 0;
        switch (type)
        {
            case LessonType.Section:
                total = dto.Sections.Count;
                break;
            case LessonType.Video:
                total = dto.Sections.SelectMany(x => x.Lessons)
                    .Count(x => x.Type == LessonType.Video);
                break;
            case LessonType.Document:
                total = dto.Sections.SelectMany(x => x.Lessons)
                    .Count(x => x.Type == LessonType.Document);
                break;
            case LessonType.Quiz:
                total = dto.Sections.SelectMany(x => x.Quizzes).Count();
                break;
        }

        return total;
    }

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
            Type = dto.Type,
            Content = dto.Content,
            Duration = dto.Duration,
            ResourceUrl = dto.ResourceUrl
        };

    public static LessonDto LessonToLessonDto(Lesson entity)
        => new LessonDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Order = entity.Order,
            Type = entity.Type.ToString(),
            Duration = entity.Duration,
            ResourceUrl = entity.ResourceUrl,
            Content = entity.Content
        };

    public static List<LessonDto> LessonToLessonDto(IEnumerable<Lesson> entities)
        => entities.Select(LessonToLessonDto).ToList();

    public static Lesson UpdateLessonDtoToLesson(Dtos.Request.Course.Update.Lesson.UpdateLessonDto dto)
        => new Lesson()
        {
            Name = dto.Name ?? throw new BadRequestException("Lesson name is required!"),
            Duration = dto.Duration,
            Content = dto.Content,
            ResourceUrl = dto.ResourceUrl,
            Type = dto.Type ?? throw new BadRequestException("Lesson type is required!")
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
    }


    public static FilterCourseDto CourseToFilterCourseDto(Course entity)
        => new FilterCourseDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            IsFree = entity.IsFree,
            PictureUrl = entity.PictureUrl
        };


    public static CommonFilterCourseDto CourseToCommonFilterCourseDto(Course entity)
        => new CommonFilterCourseDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            IsFree = entity.IsFree,
            PictureUrl = entity.PictureUrl
        };

    public static ManageFilterCourseDto CourseToManageFilterCourseDto(Course entity)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            IsFree = entity.IsFree,
            PictureUrl = entity.PictureUrl,
            Status = entity.Status.ToString(),
            CreatedDate = DateUtils.FormatDateTimeToDatetimeV1(entity.CreatedDate)
        };

    public static PagingResponse<FilterCourseDto> CourseToFilterCourseDto(PagingResponse<Course> entities)
        => new()
        {
            TotalPages = entities.TotalPages,
            TotalRecords = entities.TotalRecords,
            Results = entities.Results.Select(CourseToFilterCourseDto).ToList()
        };

    public static PagingResponse<CommonFilterCourseDto> CourseToCommonFilterCourseDto(PagingResponse<Course> entities)
        => new()
        {
            TotalPages = entities.TotalPages,
            TotalRecords = entities.TotalRecords,
            Results = entities.Results.Select(CourseToCommonFilterCourseDto).ToList()
        };

    public static PagingResponse<ManageFilterCourseDto> CourseToManageFilterCourseDto(PagingResponse<Course> entities)
        => new()
        {
            TotalPages = entities.TotalPages,
            TotalRecords = entities.TotalRecords,
            Results = entities.Results.Select(CourseToManageFilterCourseDto).ToList()
        };

    public static void UpdateCourseDtoToEntity(UpdateCourseDto dto, ref Course entity)
    {
        if (!string.IsNullOrEmpty(dto.Name))
        {
            entity.Name = dto.Name;
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

    public static CourseOrderDto ShowCoursePayment(Course dto) => new CourseOrderDto()
    {
        CourseId = dto.Id,
        TeacherId = dto.ModifiedBy?.Id,
        Picture = dto.ModifiedBy?.PictureUrl,
        CourseName = dto.Name,
        TeacherName = dto.ModifiedBy?.FullName,
        Price = dto.Price,
        ClassId = dto.Classes.FirstOrDefault()?.Id,
        ClassCode = dto.Classes.FirstOrDefault()?.Code
    };

    public static List<CourseModerationResponse> CourseToCourseModerationResponse(List<Course> dto)
    {
        return dto.Select(x => new CourseModerationResponse()
        {
            CourseId = x.Id,
            ImageUrl = x.PictureUrl,
            CourseName = x.Name,
            TeacherName = x.ModifiedBy?.FullName,
            TotalLesson = CalculateTotal(x, LessonType.Section),
            ModifiedDate = x.ModifiedDate,
            Status = x.Status
        }).ToList();
    }

    public static StudyCourseDto CourseToStudyCourseDto(Course entity)
    {
        var totalVideo = 0;
        var totalDocument = 0;
        var totalQuiz = 0;

        foreach (var entitySection in entity.Sections)
        {
            foreach (var entitySectionLesson in entitySection.Lessons)
            {
                switch (entitySectionLesson.Type)
                {
                    case LessonType.Video:
                        totalVideo++;
                        break;
                    case LessonType.Document:
                        totalDocument++;
                        break;
                    default:
                        break;
                }
            }

            totalQuiz += entitySection.Quizzes.Count;
        }

        return new StudyCourseDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            PictureUrl = entity.PictureUrl,
            IsFree = entity.IsFree,
            TotalSection = entity.Sections.Count,
            TotalDocument = totalDocument,
            TotalVideo = totalVideo,
            TotalQuiz = totalQuiz,
            Sections = entity.Sections.Select(SectionToCommonStudySectionDto).ToList()
        };
    }

    public static CommonStudyLessonDto LessonToCommonStudyLessonDto(Lesson entity)
        => new CommonStudyLessonDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Duration = entity.Duration,
            Type = entity.Type.ToString(),
            IsFree = entity.IsFree
        };

    public static CommonStudyQuizDto QuizToCommonStudyQuizDto(Quiz entity)
        => new CommonStudyQuizDto()
        {
            Id = entity.Id,
            TotalQuestion = entity.TotalQuestion,
            Duration = entity.Duration,
            Title = entity.Title
        };

    public static CommonStudySectionDto SectionToCommonStudySectionDto(Section entity)
        => new CommonStudySectionDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            SectionTime = entity.SectionTime,
            Lessons = entity?.Lessons.Select(LessonToCommonStudyLessonDto).ToList() ?? new List<CommonStudyLessonDto>(),
            Quizzes = entity?.Quizzes.Select(QuizToCommonStudyQuizDto).ToList() ?? new List<CommonStudyQuizDto>()
        };

    public static StudyLessonDto LessonToStudyLessonDto(Lesson entity)
    {
        return new StudyLessonDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Duration = entity.Duration,
            Content = entity.Content,
            ResourceUrl = entity.ResourceUrl,
            Type = entity.Type.ToString()
        };
    }
}