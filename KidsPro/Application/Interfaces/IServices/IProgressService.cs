using Application.Dtos.Response.StudentProgress;

namespace Application.Interfaces.IServices;

public interface IProgressService
{
    Task<SectionProgressResponse?> GetCourseProgressAsync(int studentId, int courseId);
    Task<List<SectionProgressResponse>?> GetStudentCoursesProgressAsync(int studentId = 0);
    Task<List<CheckProgressResponse>> CheckSectionAsync(List<int> sectionIds);
}