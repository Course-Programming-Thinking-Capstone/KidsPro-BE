using Application.Dtos.Request.Teacher;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Teacher;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class TeacherService : ITeacherService
{
    private IUnitOfWork _unitOfWork;
    private IAccountService _accountService;

    public TeacherService(IUnitOfWork unitOfWork, IAccountService accountService)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
    }

    public async Task TeacherEditProfile(ProfileRequest? profile, SocialProfileRequest? social,
        List<CertificateRequest>? certificates, EditTeacherType type)
    {
        var currentAccount = await _accountService.GetCurrentAccountInformationAsync();
        var account = await _unitOfWork.AccountRepository.GetByIdAsync(currentAccount.Id) ??
                      throw new NotFoundException("Account not found");
        var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(currentAccount.IdSubRole) ??
                      throw new NotFoundException("Teacher Profile not found");

        switch (type)
        {
            case EditTeacherType.Profile:
                account.FullName = StringUtils.FormatName(profile!.TeacherName);
                account.DateOfBirth = profile.DateOfBirth;
                account.Gender = profile.Gender > 0 ? profile.Gender : Gender.Male;
                teacher.Account = account;
                teacher.PhoneNumber = profile.PhoneNumber;
                teacher.PersonalInformation = profile.PersonalInformation;
                _unitOfWork.TeacherRepository.Update(teacher);
                break;

            case EditTeacherType.SocialProfile:
                account.Email = social!.Email;
                teacher.Account = account;
                teacher.Facebook = social.FacebookUrl;
                _unitOfWork.TeacherRepository.Update(teacher);
                break;

            case EditTeacherType.Cerificate:
                var teacherProfiles = teacher.TeacherProfiles;
                if (teacherProfiles?.Count == 0)
                    teacherProfiles = new List<TeacherProfile>();

                //Update Certifies
                foreach (var (teacherProfile, certify) in teacherProfiles!.Zip(certificates!))
                {
                    teacherProfile.CertificatePicture = certify.CertificateName;
                    teacherProfile.Description = certify.CertificateUrl;
                }

                _unitOfWork.TeacherProfileRepository.UpdateRange(teacherProfiles!);

                //Nếu bỏ bớt certificate
                if (teacherProfiles!.Count > certificates!.Count)
                {
                    var remainingTeacherCertifies
                        = teacherProfiles.Skip(certificates!.Count).ToList();
                    _unitOfWork.TeacherProfileRepository.DeleteRange(remainingTeacherCertifies);
                }

                if (teacherProfiles!.Count < certificates!.Count)
                {
                    //Add thêm teacher profile nếu ở UI add them certificate
                    var remainingCertifies
                        = certificates!.Skip(teacherProfiles.Count).ToList();
                    var teacherProfileAddRange = new List<TeacherProfile>();

                    foreach (var x in remainingCertifies)
                    {
                        var teacherProfile = new TeacherProfile()
                        {
                            CertificatePicture = x.CertificateName,
                            Description = x.CertificateUrl,
                            Teacher = teacher
                        };
                        teacherProfileAddRange.Add(teacherProfile);
                    }

                    await _unitOfWork.TeacherProfileRepository.AddRangeAsync(teacherProfileAddRange);
                }

                break;
        }

        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<List<TeacherResponse>> GetTeachers()
    {
        var teachers = await _unitOfWork.TeacherRepository.GetAllFieldAsync();
        return TeacherMapper.TeacherToTeacherResponse(teachers);
    }

    public async Task<AccountDto> GetTeacherDetail(int teacherId)
    {
        var teacher = await _unitOfWork.AccountRepository.GetTeacherAccountById(teacherId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find teacher information."));
       return  AccountMapper.AccountToTeacherDto(teacher);
    }
}