using Application.Dtos.Response.StudentProgress;

namespace Application.Interfaces.IServices;

public interface IProgressService
{
    Task<SectionProgressResponse> GetProgressSectionAync(int studentId, int courseId);
    Task<List<SectionProgressResponse>> GetStudentCourseAsync();
}