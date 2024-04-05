using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Student;

namespace Application.Interfaces.IServices;

public interface IStudentService
{
    Task<List<StudentResponse>> GetStudentsAsync(int classId=0);

    Task<StudentDetailResponse> GetDetailStudentAsync(int studentId);
    Task UpdateStudentAsync(StudentUpdateRequest dto);
}