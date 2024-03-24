using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Utils;
using Domain.Enums;

namespace Application.Services;

public class StaffService : IStaffService
{
    private IUnitOfWork _unitOfWork;
    private IAccountService _accountService;
    private INotificationService _notify;

    public StaffService(IUnitOfWork unitOfWork, IAccountService accountService, INotificationService notify)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
        _notify = notify;
    }

    public async Task CreateAccountStudentAsync(StudentCreateAccountRequest dto)
    {
        var account = await _accountService.GetCurrentAccountInformationAsync();

        if (account.Role != Constant.StaffRole)
            throw new UnauthorizedException($"Not a staff role, Please login again!");

        var order = await _unitOfWork.OrderRepository.GetByIdAsync(dto.OrderId);
        var student = await _unitOfWork.StudentRepository.GetByIdAsync(dto.StudendId);
        
        if (student == null && order == null) 
            throw new BadRequestException($"StudentId:{dto.StudendId} not found");

        var parent = await _unitOfWork.AccountRepository.ExistByEmailAsync(order!.Parent!.Account.Email!);
        if (parent == null) 
            throw new BadRequestException("Email doesn't exist");
        
        //Create student account
        student!.UserName = dto.UserName;
        student.Account.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password);
        _unitOfWork.StudentRepository.Update(student);
        //Update order status to Success
        order.Status = OrderStatus.Success;
        _unitOfWork.OrderRepository.Update(order);
        await _unitOfWork.SaveChangeAsync();
        
        //Send Email
        var toSubject = "KidsPro Send Student Account";
        var toContent = "Student Name: " + student.Account.FullName + "<br>" +
                        "Birthday: " + student.Account.DateOfBirth + "<br>" +
                        "Account: <span style='color:red;'><strong>" + dto.UserName + "</strong></span><br>" +
                        "Password: <span style='color:red;'><strong>" + dto.Password + "</strong></span>";
        EmailUtils.SendEmail(parent.Email!, toSubject, toContent);
        
        //Send Notify
        var title = "The order "+order.OrderCode+" has been successfully confirmed";
        var content = "Student account of "+student.Account.FullName+" send to email "+parent.Email;
        await _notify.SendNotifyToAccountAsync(order.ParentId, title, content);
    }

    public async Task<string> ViewReasonOrderCancel(int orderId, int parentId)
    {
        // Check và lấy order vs requestRefund status
        var order = await _unitOfWork.OrderRepository.GetOrderByStatusAsync(parentId, orderId,
            OrderStatus.RequestRefund);
        return order?.Note ?? throw new BadRequestException($"OrderId: {orderId} not RequestRefund Status");
    }
}