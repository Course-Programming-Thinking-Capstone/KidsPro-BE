using Application.Dtos.Response.StudentProgress;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities;

namespace Application.Services;

public class ProgressService : IProgressService
{
    private IUnitOfWork _unit;
    private IAccountService _account;

    public ProgressService(IUnitOfWork unit, IAccountService accountService)
    {
        _unit = unit;
        _account = accountService;
    }

    private async Task<List<StudentProgress>> GetProgressListAsync(int studentId, int courseId = 0)
    {
        return await _unit.StudentProgressRepository.GetSectionProgress(studentId, courseId)
               ?? throw new NotFoundException($"StudentId {studentId} or CourseId {courseId} not found");
    }

    public async Task<SectionProgressResponse?> GetCourseProgressAsync(int studentId, int courseId)
    {
        var student = await GetProgressListAsync(studentId, courseId);

        if (student.Count == 0) return null;

        return ProgressMapper.StudentToProgressResponse(student);
    }

    public async Task<List<SectionProgressResponse>?> GetStudentCoursesProgressAsync()
    {
        var account = await _account.GetCurrentAccountInformationAsync();

        var student = await GetProgressListAsync(account.IdSubRole);
        if (student.Count == 0) return null;

        return ProgressMapper.StudentToProgressResponseList(student);
    }
}