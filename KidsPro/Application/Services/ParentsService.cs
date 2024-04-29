using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Parent;
using Application.Dtos.Response.Account.Student;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ParentsService : IParentsService
{
    private readonly IUnitOfWork _unitOfWork;
    private IAccountService _accountService;

    public ParentsService(IUnitOfWork unitOfWork, IAccountService accountService)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
    }

    public async Task<StudentResponse> AddStudentAsync(StudentAddRequest request)
    {
        var studentRole = await _unitOfWork.RoleRepository.GetByNameAsync(Constant.StudentRole)
            .ContinueWith(t => t.Result ?? throw new Exception("Role student name is incorrect."));

        var account = await _accountService.GetCurrentAccountInformationAsync();

        var accountEntity = new Account()
        {
            PasswordHash = "000000",
            FullName = StringUtils.FormatName(request.FullName),
            Role = studentRole,
            DateOfBirth = request.Birthday,
            Gender = (Gender)(request.Gender > 0 ? request.Gender : 1),
            CreatedDate = DateTime.UtcNow,
            Status = UserStatus.Active
        };

        var studentEntity = new Student()
        {
            ParentId = account.IdSubRole,
            Account = accountEntity
        };

        await _unitOfWork.StudentRepository.AddAsync(studentEntity);
        await _unitOfWork.SaveChangeAsync();

        var result = AccountMapper.AccountToStudentDto(accountEntity);
        return result;
    }

    public async Task<ParentOrderResponse> GetParentEmail()
    {
        var account = await _accountService.GetCurrentAccountInformationAsync();
        var result = _unitOfWork.ParentRepository.GetEmailZalo(account.IdSubRole);
        if (result != null)
            return ParentMapper.ParentShowContact(result);
        throw new NotFoundException("Parent doesn't exist");
    }

    public async Task<List<GameVoucher>?> GetListVoucherAsync(VoucherStatus status)
    {
        var account = await _accountService.GetCurrentAccountInformationAsync();
        var vouchers = await _unitOfWork.VoucherRepository.GetListVoucher(account.IdSubRole, status);
        return vouchers;
    }
}