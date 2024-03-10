using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Lesson;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.Lesson;
using Application.ErrorHandlers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappers;

public static class CourseMapper
{
    public static Course CreateCourseDtoToEntity(CreateCourseDto dto)
        => new Course()
        {
            Name = dto.Name,
            Description = dto.Description
        };

    public static SectionDto SectionToSectionDto(Section entity)
        => new SectionDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Order = entity.Order,
            CourseId = entity.CourseId,
            Lessons = entity.Lessons.Select(LessonToLessonDto).ToList()
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
    {
        return dto switch
        {
            CreateVideoDto createVideoDto => new Lesson()
            {
                Name = createVideoDto.Name,
                Order = createVideoDto.Order,
                IsFree = createVideoDto.IsFree,
                Type = LessonType.Video,
                Duration = createVideoDto.Duration,
                ResourceUrl = createVideoDto.ResourceUrl
            },
            CreateDocumentDto createDocumentDto => new Lesson()
            {
                Name = createDocumentDto.Name,
                Order = createDocumentDto.Order,
                IsFree = createDocumentDto.IsFree,
                Type = LessonType.Document,
                Duration = createDocumentDto.Duration,
                Content = createDocumentDto.Content
            },
            _ => throw new UnsupportedException("Unsupported lesson type.")
        };
    }

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

    public static List<LessonDto> LessonToLessonDto(IEnumerable<Lesson> entities)
        => entities.Select(LessonToLessonDto).ToList();

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