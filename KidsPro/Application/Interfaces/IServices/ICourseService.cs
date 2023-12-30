using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;

namespace Application.Interfaces.IServices;

public interface ICourseService
{
    Task<CourseDto> CreateAsync(CreateCourseDto request);

    Task<CourseDto> UpdateAsync(int id, UpdateCourseDto request);
}