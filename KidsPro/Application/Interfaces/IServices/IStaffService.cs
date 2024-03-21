using Application.Dtos.Request.Account.Student;

namespace Application.Interfaces.IServices;

public interface IStaffService
{
    Task CreateAccountStudentAsync(StudentCreateAccountRequest dto);
    Task<string> ViewReasonOrderCancel(int orderId, int parentId);
}