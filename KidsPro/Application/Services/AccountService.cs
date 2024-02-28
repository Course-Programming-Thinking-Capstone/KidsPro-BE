using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.Account;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Constant = Application.Configurations.Constant;

namespace Application.Services;

public class AccountService : IAccountService
{
    private IUnitOfWork _unitOfWork;
    private IAuthenticationService _authenticationService;

    public AccountService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
    }

    public async Task<LoginAccountDto> RegisterByEmailAsync(EmailRegisterDto dto)
    {
        if (await _unitOfWork.AccountRepository.ExistByEmailAsync(dto.Email))
            throw new ConflictException($"Email {dto.Email} has been existed.");

        var parentRole = await _unitOfWork.RoleRepository.GetByNameAsync(Constant.ParentRole)
            .ContinueWith(t => t.Result ?? throw new Exception("Role parent name is incorrect."));

        var accountEntity = new Account()
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password),
            FullName = StringUtils.FormatName(dto.FullName),
            Role = parentRole
        };

        var parentEntity = new Parent()
        {
            Account = accountEntity
        };

        await _unitOfWork.ParentRepository.AddAsync(parentEntity);
        await _unitOfWork.SaveChangeAsync();

        var result = AccountMapper.EntityToLoginAccountDto(accountEntity);
        result.AccessToken = _authenticationService.CreateAccessToken(accountEntity);
        result.RefreshToken = _authenticationService.CreateRefreshToken(accountEntity);
        return result;
    }
}