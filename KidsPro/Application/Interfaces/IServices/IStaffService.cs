using Application.Dtos.Request.Account.Student;
using Application.Dtos.Request.Email;

namespace Application.Interfaces.IServices;

public interface IStaffService
{
    Task<EmailContentRequest> CreateAccountStudentAsync(StudentCreateAccountRequest dto);
    Task<string> ViewReasonOrderCancel(int orderId);
    Task SendEmailParentAsync(EmailContentRequest dto);
}