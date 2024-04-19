using Application.Dtos.Response.CourseGame;
using Domain.Entities;

namespace Application.Mappers;

public static class CourseGameMapper
{
    public static CourseGameDto CourseGameToCourseGameDto(CourseGame entity)
        => new CourseGameDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Url = entity.Url,
            Status = entity.Status.ToString()
        };
}