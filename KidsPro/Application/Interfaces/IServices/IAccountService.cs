using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.Account;

namespace Application.Interfaces.IServices;

public interface IAccountService
{
    Task<LoginAccountDto> RegisterByEmailAsync(EmailRegisterDto dto);

    Task<LoginAccountDto> RegisterByPhoneNumberAsync(PhoneNumberRegisterDto dto);

    Task<LoginAccountDto> LoginByEmailAsync(EmailCredential dto);

    Task<LoginAccountDto> LoginByPhoneNumberAsync(PhoneCredential dto);

    Task ChangePasswordAsync(string oldPassword, string newPassword);

}