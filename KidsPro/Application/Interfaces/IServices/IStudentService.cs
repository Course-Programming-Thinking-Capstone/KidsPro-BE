using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Student;

namespace Application.Interfaces.IServices;

public interface IStudentService
{
    Task<List<StudentResponse>> GetStudentsAsync();

    Task<StudentDetailResponse> GetDetailStudentAsync(int studentId);
    Task UpdateStudentAsync(StudentUpdateRequest dto);
}