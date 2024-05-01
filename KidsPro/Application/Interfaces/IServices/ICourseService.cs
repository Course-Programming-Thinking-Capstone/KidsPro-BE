using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Request.Progress;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.CourseModeration;
using Application.Dtos.Response.Course.FilterCourse;
using Application.Dtos.Response.Course.Quiz;
using Application.Dtos.Response.Course.Quiz.QuizDetail;
using Application.Dtos.Response.Course.Study;
using Application.Dtos.Response.Paging;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface ICourseService
{
    Task<CourseDto> GetByIdAsync(int id, string? action);

    Task<StudyCourseDto?> GetActiveStudyCourseByIdAsync(int id);

    Task<StudyCourseDto?> GetStudyCourseByIdAsync(int id);

    Task<CommonStudySectionDto?> GetActiveCourseStudySectionByIdAsync(int id);
    
    //Admin/staff view section detail
    Task<CommonStudySectionDto?> GetSectionDetailByIdAsync(int sectionId);

    Task<StudyLessonDto?> GetFreeStudyLessonByIdAsync(int id);
    
    Task<StudyLessonDto?> GetStudyLessonByIdAsync(int lessonId);

    Task<QuizDetailDto> GetQuizByIdAsync(int quizId);

    Task<CourseDto> CreateCourseAsync(CreateCourseDto dto);

    Task<CourseDto> UpdateCourseAsync(int courseId, Dtos.Request.Course.Update.Course.UpdateCourseDto dto,
        string? action);

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
        bool? isFree,
        int? page,
        int? size
    );

    Task<IPagingResponse<ManageFilterCourseDto>> FilterTeacherCourseAsync(
        string? name,
        CourseStatus? status,
        int? page,
        int? size
    );

    Task<ICollection<SectionComponentNumberDto>> GetSectionComponentNumberAsync();

    Task<ICollection<SectionComponentNumberDto>> UpdateSectionComponentNumberAsync(
        IEnumerable<UpdateSectionComponentNumberDto> dtos);

    Task<CourseOrderDto> GetCoursePaymentAsync(int courseId, int classId);
    Task StartStudyCourseAsync(StudentProgressRequest dto ,List<int>? studentIds = null);
    Task MarkLessonCompletedAsync(int lessonId);
    Task UpdateToPendingStatus(int courseId, int number);
    Task<List<CourseModerationResponse>> GetCourseModerationAsync();
}