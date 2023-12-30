using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

public class UserMapper:Profile
{
    public static LoginUserDto EntityToLoginUserDto(User entity)
    {
        return new LoginUserDto()
        {
            Id = entity.Id,
            PhoneNumber = entity.PhoneNumber,
            FullName = entity.FullName,
            PictureUrl = entity.PictureUrl,
            Gender = entity.Gender?.ToString(),
            Role = entity.Role.Name,
        };
    }

    public UserMapper()
    {
        //UserResponse
        CreateMap<User,UserResponseDto>()
            .ForMember(x=> x.RoleName, otp =>otp.MapFrom(src => src.Role != null ? src.Role.Name: string.Empty))
            .ReverseMap();
    }
}