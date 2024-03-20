using Application.Dtos.Request.Account.Student;

namespace Application.Interfaces.IServices;

public interface IStaffService
{
    Task CreateAccountStudentAsync(StudentCreateAccountRequest dto);
}