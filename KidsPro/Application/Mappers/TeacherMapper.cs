using Application.Dtos.Response.Teacher;
using Domain.Entities;
using Google.Apis.Util;

namespace Application.Mappers;

public static class TeacherMapper
{
    public static List<TeacherResponse> TeacherToTeacherResponse(List<Teacher> dto)
    {
        return dto.Select(x => new TeacherResponse
        {
            TeacherId = x.Account.Id,
            TeacherName = x.Account.FullName,
            TeacherPicture = x.Account.PictureUrl,
            PersonalInformation = x.PersonalInformation
        }).ToList();
    }
    
}