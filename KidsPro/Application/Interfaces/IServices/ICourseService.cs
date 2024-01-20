using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.Section;
using Application.Dtos.Response.Paging;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface ICourseService
{
    Task<CourseDto> CreateAsync(CreateCourseDto request);

    Task<CourseDto> UpdateAsync(int id, UpdateCourseDto request);

    Task<CourseDto> UpdatePictureAsync(int courseId, IFormFile file);

    Task DeleteAsync(int id);

    Task<PagingResponse<CommonManageCourseDto>> GetManageCourseAsync(
        string? name,
        string? status,
        string? sortName,
        string? sortCreatedDate,
        string? sortModifiedDate,
        int? page,
        int? size,
        bool isOfCurrentUser = false
    );

    Task<PagingResponse<CommonCourseDto>> GetCoursesAsync(
        string? name,
        string? sortName,
        string? sortPostedDate,
        int? page,
        int? size
    );

    Task<CourseDto> AddSectionAsync(int courseId, string sectionName);

    Task<CourseDto> RemoveSectionAsync(int courseId, int courseSectionId);

    Task<CourseDto> UpdateSectionOrderAsync(int courseId, List<SectionDto> sections);
}