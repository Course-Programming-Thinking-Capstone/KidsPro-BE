using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface ICourseService
{
    Task<CourseDto> CreateAsync(CreateCourseDto request);

    Task<CourseDto> UpdateAsync(int id, UpdateCourseDto request);

    Task<CourseDto> UpdatePictureAsync(int courseId, IFormFile file);
}