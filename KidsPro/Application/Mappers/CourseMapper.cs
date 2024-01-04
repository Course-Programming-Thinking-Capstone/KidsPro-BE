using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;
using Application.Utils;
using Domain.Entities;

namespace Application.Mappers;

public class CourseMapper
{
    public static CourseDto EntityToDto(Course entity)
    {
        return new CourseDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Prerequisite = entity.Prerequisite,
            FromAge = entity.FromAge,
            ToAge = entity.ToAge,
            PictureUrl = entity.PictureUrl,
            OpenDate = DateUtil.FormatDateTimeToDatetimeV1(entity.OpenDate),
            StartSaleDate = DateUtil.FormatDateTimeToDatetimeV1(entity.StartSaleDate),
            EndSaleDate = DateUtil.FormatDateTimeToDatetimeV1(entity.EndSaleDate),
            Price = entity.Price,
            DiscountPrice = entity.DiscountPrice,
            TotalLesson = entity.TotalLesson,
            Status = entity.Status.ToString(),
            CreatedDate = DateUtil.FormatDateTimeToDatetimeV1(entity.CreatedDate),
            ModifiedDate = DateUtil.FormatDateTimeToDatetimeV1(entity.ModifiedDate),
            ModifiedById = entity.ModifiedById,
            ModifiedByName = entity.ModifiedBy.FullName,
            CreatedById = entity.CreatedById,
            CreatedByName = entity.CreatedBy.FullName,
            Resources = entity.CourseResources?.Select(CourseResourceMapper.EntityToDto).ToList()
        };
    }

    public static Course CreateDtoToEntity(CreateCourseDto dto)
    {
        return new Course()
        {
            Name = dto.Name,
            Description = dto.Description,
            Prerequisite = dto.Prerequisite,
            FromAge = dto.FromAge,
            ToAge = dto.ToAge,
            CourseResources = dto.Resources?.Select(CourseResourceMapper.AddDtoToEntity).ToList()
        };
    }

    public static void UpdateDtoToEntity(UpdateCourseDto dto, ref Course entity)
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
            entity.Prerequisite = dto.Prerequisite;
        }

        if (dto.FromAge.HasValue)
        {
            entity.FromAge = dto.FromAge.Value;
        }

        if (dto.ToAge.HasValue)
        {
            entity.ToAge = dto.ToAge;
        }
    }

    private static string FormatAgeString(int? fromAge, int? toAge)
    {
        if (fromAge.HasValue && toAge.HasValue)
        {
            return $"{fromAge.Value} - {toAge.Value}";
        }

        if (fromAge.HasValue && !toAge.HasValue)
        {
            return $"{fromAge}+";
        }

        if (!fromAge.HasValue && toAge.HasValue)
        {
            return $"{toAge}";
        }

        return string.Empty;
    }
}