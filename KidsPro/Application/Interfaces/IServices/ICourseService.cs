using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Lesson;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.Lesson;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface ICourseService
{
    Task<CourseDto> GetByIdAsync(int id);

    Task<CourseDto> CreateCourseAsync(CreateCourseDto dto);

    Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto dto);

    Task<string> UpdateCoursePictureAsync(int id, IFormFile file);

    Task<SectionDto> CreateSectionAsync(int courseId, CreateSectionDto dto);
    Task<SectionDto> UpdateSectionAsync(int sectionId, UpdateSectionDto dto);
    Task<List<SectionDto>> UpdateSectionOrderAsync(int courseId, List<UpdateSectionOrderDto> dtos);

    Task<ICollection<SectionComponentNumberDto>> GetSectionComponentNumberAsync();

    Task<ICollection<SectionComponentNumberDto>> UpdateSectionComponentNumberAsync(
        List<UpdateSectionComponentNumberDto> dtos);

    Task RemoveSectionAsync(int id);

    Task<LessonDto> AddVideoAsync(int sectionId, CreateVideoDto dto);
}