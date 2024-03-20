using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Utils;

namespace Application.Services;

public class StaffService:IStaffService
{
    private IUnitOfWork _unitOfWork;
    private IAccountService _accountService;

    public StaffService(IUnitOfWork unitOfWork, IAccountService accountService)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
    }

    public async Task CreateAccountStudentAsync(StudentCreateAccountRequest dto)
    {
        var account = await _accountService.GetCurrentAccountInformationAsync();

        if (account.Role == Constant.StaffRole)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(dto.StudendId);
            if (student != null)
            {
                student.UserName = dto.UserName;
                student.Account.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password);
                _unitOfWork.StudentRepository.Update(student);
                var result= await _unitOfWork.SaveChangeAsync();
                if (result > 0 &&
                    await _unitOfWork.AccountRepository.ExistByEmailAsync(dto.ParentEmail!))
                {
                    //Send Email
                    var toSubject = "KidsPro Send Student Account";
                    var toContent = "Student Name: " + student.Account.FullName + "<br>" + 
                                    "Birthday: " + student.Account.DateOfBirth + "<br>" +
                                    "Account: <span style='color:red;'><strong>" + dto.UserName + "</strong></span><br>" + 
                                    "Password: <span style='color:red;'><strong>" + dto.Password + "</strong></span>";
                    EmailUtils.SendEmail(dto.ParentEmail!, toSubject, toContent);
                    return;
                }

                throw new BadRequestException("Email doesn't exist");
            }
            throw new BadRequestException($"StudentId:{dto.StudendId} not found");
        }
        throw new UnauthorizedException($"Not a staff role, Please login again!");
    }
    
    
    
}