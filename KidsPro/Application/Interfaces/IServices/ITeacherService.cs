using Application.Dtos.Request.Teacher;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Teacher;
using Domain.Enums;

namespace Application.Interfaces.IServices;

public interface ITeacherService
{
    Task TeacherEditProfile(ProfileRequest? profile, SocialProfileRequest? social,
        List<CertificateRequest>? certificates, EditTeacherType type);

    Task<List<TeacherResponse>> GetTeachers();
    Task<AccountDto> GetTeacherDetail(int teacherId);
}