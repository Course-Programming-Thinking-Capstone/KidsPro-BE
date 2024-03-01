using Application.Dtos.Response.Account;
using Domain.Entities;

namespace Application.Mappers;

public static class AccountMapper
{
    public static LoginAccountDto EntityToLoginAccountDto(Account entity)
        => new LoginAccountDto()
        {
            Id = entity.Id,
            Email = entity.Email,
            FullName = entity.FullName,
            PictureUrl = entity.PictureUrl,
            Role = entity.Role.Name
        };


    public static StudentGameLoginDto StudentToStudentGameLoginDto(Student entity)
        => new StudentGameLoginDto()
        {
            UserId = entity.Id,
            DisplayName = entity.GameUserProfile.DisplayName,
            UserCoin = entity.GameUserProfile.Coin,
            UserGem = entity.GameUserProfile.Gem
        };
}