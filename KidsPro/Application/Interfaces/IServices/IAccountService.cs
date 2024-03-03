using Application.Dtos.Request.Authentication;
using Application.Dtos.Request.User;
using Application.Dtos.Response.Account;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface IAccountService
{
    Task<LoginAccountDto> RegisterByEmailAsync(EmailRegisterDto dto);

    Task<LoginAccountDto> RegisterByPhoneNumberAsync(PhoneNumberRegisterDto dto);

    Task<LoginAccountDto> LoginByEmailAsync(EmailCredential dto);

    Task<LoginAccountDto> LoginByPhoneNumberAsync(PhoneCredential dto);

    Task ChangePasswordAsync(ChangePasswordDto dto);
    Task<string> UpdatePictureAsync(IFormFile file);

    Task<StudentGameLoginDto> StudentGameLoginAsync(EmailCredential dto);

    Task<AccountDto> GetCurrentAccountInformationAsync();
}