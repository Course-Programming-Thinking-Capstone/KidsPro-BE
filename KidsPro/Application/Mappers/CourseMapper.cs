using Application.Dtos.Request.Course;
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
            Version = entity.Version,
            Name = entity.Name,
            Order = entity.Order
        };

    public static CourseDto CourseToCourseDto(Course entity)
        => new CourseDto()
        {
            Id = entity.Id,
            Version = entity.Version,
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
}