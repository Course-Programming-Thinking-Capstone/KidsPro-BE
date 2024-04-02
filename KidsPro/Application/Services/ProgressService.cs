using Application.Dtos.Response.StudentProgress;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;

namespace Application.Services;

public class ProgressService:IProgressService
{
    private IUnitOfWork _unit;

    public ProgressService(IUnitOfWork unit)
    {
        _unit = unit;
    }

    public async Task<SectionProgressResponse> GetProgressSection(int studentId,int courseId)
    {
        var student = await _unit.StudentProgressRepository.GetSectionProgress(studentId,courseId)
                      ??throw new BadRequestException($"StudentId {studentId} or CourseId {courseId} not found");

        return ProgressMapper.StudentToProgressResponse(student);
    }
}