using Application.Interfaces.IServices;

namespace Application.Services;

public class StudentProgressService:IStudentProgressService
{
    private IUnitOfWork _unit;

    public StudentProgressService(IUnitOfWork unit)
    {
        _unit = unit;
    }

    // public async Task GetProgressSection(int studentId)
    // {
    //     var stu
    // }
}