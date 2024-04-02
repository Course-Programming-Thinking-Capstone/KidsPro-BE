using Application.Dtos.Response.StudentProgress;

namespace Application.Interfaces.IServices;

public interface IProgressService
{
    Task<SectionProgressResponse> GetProgressSection(int studentId, int courseId);
}