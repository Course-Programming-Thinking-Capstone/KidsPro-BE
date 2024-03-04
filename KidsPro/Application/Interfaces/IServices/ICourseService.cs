using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;

namespace Application.Interfaces.IServices;

public interface ICourseService
{
    Task<CourseDto> GetByIdAsync(int id);

    Task<CourseDto> CreateCourseAsync(CreateCourseDto dto);
}