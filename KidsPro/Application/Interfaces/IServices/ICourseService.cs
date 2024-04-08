using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Request.Progress;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.FilterCourse;
using Application.Dtos.Response.Paging;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface ICourseService
{
    Task<CourseDto> GetByIdAsync(int id, string? action);

    Task<CourseDto> CreateCourseAsync(CreateCourseDto dto);

    Task<CourseDto> UpdateCourseAsync(int id, Dtos.Request.Course.Update.Course.UpdateCourseDto dto, string? action);

    Task ApproveCourseAsync(int id, AcceptCourseDto to);

    Task DenyCourseAsync(int id, string? reason);

    Task<CourseDto> CommonUpdateCourseAsync(int id, UpdateCourseDto dto);

    Task<string> UpdateCoursePictureAsync(int id, IFormFile file);

    Task<IPagingResponse<FilterCourseDto>> FilterCourseAsync(
        string? name,
        CourseStatus? status,
        string? sortName,
        string? sortCreatedDate,
        string? sortModifiedDate,
        string? action,
        int? page,
        int? size
    );

    Task<ICollection<SectionComponentNumberDto>> GetSectionComponentNumberAsync();

    Task<ICollection<SectionComponentNumberDto>> UpdateSectionComponentNumberAsync(
        IEnumerable<UpdateSectionComponentNumberDto> dtos);

    Task<CourseOrderDto> GetCoursePaymentAsync(int courseId, int classId);
    Task StartStudySectionAsync(StudentProgressRequest dto);
    Task MarkLessonCompletedAsync(int lessonId);
}