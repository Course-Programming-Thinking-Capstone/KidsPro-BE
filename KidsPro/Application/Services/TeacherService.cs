using Application.Dtos.Request.Teacher;
using Application.Interfaces.IServices;
using Domain.Entities;

namespace Application.Services;

public class TeacherService
{
    private IUnitOfWork _unitOfWork;
    private IAccountService _accountService;

    public TeacherService(IUnitOfWork unitOfWork, IAccountService accountService)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
    }

    // public async Task TeacherEditProfile(EditProfileRequest dto)
    // {
    //     var account = await _accountService.GetCurrentAccountInformationAsync();
    //     var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(account.Id);
    //     var updateTeacher= new Account
    //     {
    //         FullName = dto.TeacherName!=null?dto.TeacherName:teacher.n,
    //         
    //     }
    // }
}