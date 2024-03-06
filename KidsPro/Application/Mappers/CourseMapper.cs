using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.Utils;
using Domain.Entities;

namespace Application.Mappers;

public static class CourseMapper
{
    public static Course CreateCourseDtoToEntity(CreateCourseDto dto)
        => new Course()
        {
            Name = dto.Name,
            Description = dto.Description,
            PreRequire = dto.Prerequisite,
            FromAge = dto.FromAge,
            ToAge = dto.ToAge
        };

    public static SectionDto SectionToSectionDto(Section entity)
        => new SectionDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Order = entity.Order,
            CourseId = entity.CourseId
        };

    public static List<SectionDto> SectionToSectionDto(IEnumerable<Section> entities)
        => entities.Select(SectionToSectionDto).ToList();

    public static CourseDto CourseToCourseDto(Course entity)
        => new CourseDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            FromAge = entity.FromAge,
            ToAge = entity.ToAge,
            Prerequisite = entity.PreRequire,
            Price = entity.Price,
            CreatedDate = DateUtils.FormatDateTimeToDatetimeV1(entity.CreatedDate),
            PictureUrl = entity.PictureUrl,
            Status = entity.Status.ToString(),
            DiscountPrice = entity.DiscountPrice,
            Language = entity.Language,
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

        if (!string.IsNullOrEmpty(dto.Prerequisite))
        {
            entity.PreRequire = dto.Prerequisite;
        }

        if (!string.IsNullOrEmpty(dto.Language))
        {
            entity.Language = dto.Language;
        }

        if (!string.IsNullOrEmpty(dto.GraduateCondition))
        {
            entity.GraduateCondition = dto.GraduateCondition;
        }

        if (dto.FromAge.HasValue)
        {
            entity.FromAge = dto.FromAge.Value;
        }

        if (dto.ToAge.HasValue)
        {
            entity.ToAge = dto.ToAge.Value;
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
}