using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.User;
using Domain.Entities;

namespace Application.Mappers;

public class UserMapper
{
    public static LoginUserDto EntityToLoginUserDto(User entity)
    {
        return new LoginUserDto()
        {
            PhoneNumber = entity.PhoneNumber,
            FullName = entity.FullName,
            PictureUrl = entity.PictureUrl,
            Gender = entity.Gender?.ToString(),
            Role = entity.Role.Name,
        };
    }
}