using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Lesson;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.Lesson;
using Application.Dtos.Response.Paging;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface ICourseService
{
    Task<CourseDto> GetByIdAsync(int id);

    Task<CourseDto> CreateCourseOldAsync(CreateCourseDto dto);

    Task<CourseDto> CreateCourseAsync(CreateCourseDto dto);

    Task<CourseDto> UpdateCourseAsync(int id, Dtos.Request.Course.Update.Course.UpdateCourseDto dto);

    Task ApproveCourseAsync(int id);

    Task DenyCourseAsync(int id, string? reason);

    Task<CourseDto> CommonUpdateCourseAsync(int id, UpdateCourseDto dto);

    Task<string> UpdateCoursePictureAsync(int id, IFormFile file);

    Task<PagingResponse<FilterCourseDto>> FilterCourseAsync(string? name, CourseStatus? status, string? sortName,
        int? page, int? size);

    Task<SectionDto> CreateSectionAsync(int courseId, CreateSectionDto dto);
    Task<SectionDto> UpdateSectionAsync(int sectionId, UpdateSectionDto dto);
    Task<List<SectionDto>> UpdateSectionOrderAsync(int courseId, List<UpdateSectionOrderDto> dtos);

    Task<ICollection<SectionComponentNumberDto>> GetSectionComponentNumberAsync();

    Task<ICollection<SectionComponentNumberDto>> UpdateSectionComponentNumberAsync(
        List<UpdateSectionComponentNumberDto> dtos);

    Task RemoveSectionAsync(int id);

    // Task<LessonDto> AddVideoAsync(int sectionId, CreateVideoDto dto);
    // Task<LessonDto> AddDocumentAsync(int sectionId, CreateDocumentDto dto);

    Task<LessonDto> UpdateVideoAsync(int videoId, UpdateVideoDto dto);
    Task<LessonDto> UpdateDocumentAsync(int documentId, UpdateDocumentDto dto);

    Task<ICollection<LessonDto>> UpdateLessonOrderAsync(List<UpdateLessonOrderDto> dtos);
}