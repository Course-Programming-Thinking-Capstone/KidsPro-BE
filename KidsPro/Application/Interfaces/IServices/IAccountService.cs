using Application.Dtos.Request.Account;
using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Paging;
using Domain.Entities;
using Domain.Enums;
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

    Task<StudentGameLoginDto> StudentGameLoginAsync(StudentLoginRequest dto);

    Task<AccountDto> GetCurrentAccountInformationAsync();

    Task<AccountDto> GetAccountByIdAsync(int id, string role);

    Task<AccountDto> CreateAccountAsync(CreateAccountDto dto);

    Task<PagingResponse<AccountDto>> FilterAccountAsync(
        string? fullName,
        Gender? gender,
        string? role,
        string? status,
        string? sortFullName,
        string? sortCreatedDate,
        int? page, int? size
    );

    Task CheckConfirmation(string input);
    Task SendConfirmation();
    Task UpdateToNotActivatedStatus(string email);
    Task<LoginAccountDto> StudentLoginToWeb(StudentLoginRequest dto);
    Task UpdateUserStatusAsync(int id, UserStatus status);

}