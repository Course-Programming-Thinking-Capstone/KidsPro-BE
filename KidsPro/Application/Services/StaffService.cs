using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.Dtos.Request.Class;
using Application.Dtos.Request.Email;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Enums;

namespace Application.Services;

public class StaffService : IStaffService
{
    private IUnitOfWork _unitOfWork;
    private IAccountService _accountService;
    private INotificationService _notify;
    private IClassService _classes;

    public StaffService(IUnitOfWork unitOfWork, IAccountService accountService, INotificationService notify,
        IClassService classes)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
        _notify = notify;
        _classes = classes;
    }

    public async Task<EmailContentRequest> CreateAccountStudentAsync(StudentCreateAccountRequest dto)
    {
        var account = await _accountService.GetCurrentAccountInformationAsync();

        if (account.Role != Constant.StaffRole)
            throw new UnauthorizedException($"Not a staff role, Please login again!");

        var order = await _unitOfWork.OrderRepository.GetByIdAsync(dto.OrderId)
                    ?? throw new BadRequestException($"OrderId:{dto.OrderId} not found");

        var student = await _unitOfWork.StudentRepository.GetByIdAsync(dto.StudendId)
                      ?? throw new BadRequestException($"StudentId:{dto.StudendId} not found");

        var entityClass = await _unitOfWork.ClassRepository.GetByIdAsync(dto.ClassId)
                          ?? throw new BadRequestException($"ClassId:{dto.ClassId} not found");

        var checkNameOverlap = await _unitOfWork.StudentRepository.CheckNameOverlapAsync(dto.UserName);
        if (checkNameOverlap!=null) throw new ConflictException("Username already exists");
        
        //Create student account
        student!.UserName = dto.UserName;
        student.Account.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password);
        _unitOfWork.StudentRepository.Update(student);

        //Add student to Class
        var studentsAdd = new StudentsAddRequest
        {
            ClassId = dto.ClassId,
            StudentIds = entityClass.Students.Select(x => x.Id).ToList()
        };
        studentsAdd.StudentIds.Add(student.Id);

        await _classes.UpdateStudentsToClassAsync(studentsAdd);

        return EmailMapper.ShowEmailContentResponse(student, dto.Password!, order);
    }

    public async Task<string> ViewReasonOrderCancel(int orderId)
    {
        // Check và lấy order vs requestRefund status
        var order = await _unitOfWork.OrderRepository.GetOrderByStatusAsync(orderId,
            OrderStatus.RequestRefund);
        return order?.Note ?? throw new BadRequestException($"OrderId: {orderId} not found");
    }

    public async Task SendEmailParentAsync(EmailContentRequest dto)
    {
        //Send Email
        var toSubject = "KidsPro Send Student Account";
        var toContent = "Student Name: " + dto.StudentName + "<br>" +
                        "Birthday: " + dto.Birthday + "<br>" +
                        "Account: <span style='color:red;'><strong>" + dto.Account + "</strong></span><br>" +
                        "Password: <span style='color:red;'><strong>" + dto.Password + "</strong></span><br>" +
                        "Note: " + dto.Note;
        EmailUtils.SendEmail(dto.Email!, toSubject, toContent);

        //Send Notify
        var title = "The order has been successfully confirmed";
        var content = "Student account of " + dto.StudentName + " send to email " + dto.Email;
        await _notify.SendNotifyToAccountAsync(dto.ParentId, title, content);
    }
}