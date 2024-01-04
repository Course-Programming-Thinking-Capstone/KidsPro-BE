using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;
using Domain.Entities;

namespace Application.Mappers;

public class CourseResourceMapper
{
    public static CourseResource AddDtoToEntity(AddCourseResourceDto dto)
    {
        return new CourseResource()
        {
            Description = dto.Description,
            Title = dto.Title,
            ResourceUrl = dto.ResourceUrl
        };
    }

    public static CourseResourceDto EntityToDto(CourseResource entity)
    {
        return new CourseResourceDto()
        {
            Description = entity.Description,
            Title = entity.Title,
            ResourceUrl = entity.ResourceUrl
        };
    }
}