using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.Account;

namespace Application.Interfaces.IServices;

public interface IAccountService
{
    Task<LoginAccountDto> RegisterByEmailAsync(EmailRegisterDto dto);

}