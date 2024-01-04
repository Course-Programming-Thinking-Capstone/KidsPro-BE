using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Paging;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface ICourseService
{
    Task<CourseDto> CreateAsync(CreateCourseDto request);

    Task<CourseDto> UpdateAsync(int id, UpdateCourseDto request);

    Task<CourseDto> UpdatePictureAsync(int courseId, IFormFile file);

    Task DeleteAsync(int id);

    Task<PagingResponse<CommonCourseDto>> GetCourseAsync(
        string? name,
        string? status,
        string? sortName,
        string? sortCreatedDate,
        string? sortModifiedDate,
        int? page,
        int? size
    );
}