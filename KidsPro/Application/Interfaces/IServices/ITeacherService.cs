using Application.Dtos.Request.Teacher;
using Domain.Enums;

namespace Application.Interfaces.IServices;

public interface ITeacherService
{
    Task TeacherEditProfile(ProfileRequest? profile, SocialProfileRequest? social,
        List<CertificateRequest>? certificates, EditTeacherType type);
    
}