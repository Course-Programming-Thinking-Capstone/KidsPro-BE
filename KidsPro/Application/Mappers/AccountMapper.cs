using Application.Dtos.Response.Account;
using Domain.Entities;

namespace Application.Mappers;

public static class AccountMapper
{
    public static LoginAccountDto EntityToLoginAccountDto(Account entity)
        => new LoginAccountDto()
        {
            Email = entity.Email,
            FullName = entity.FullName,
            PictureUrl = entity.PictureUrl,
            Role = entity.Role.Name
        };
}