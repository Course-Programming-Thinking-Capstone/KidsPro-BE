using Application.Dtos.Request.Authentication;
using Application.Dtos.Request.User;
using Application.Dtos.Response.Account;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Constant = Application.Configurations.Constant;

namespace Application.Services;

public class AccountService : IAccountService
{
    private IUnitOfWork _unitOfWork;
    private IAuthenticationService _authenticationService;
    private IImageService _imageService;


    public AccountService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService,
        IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
        _imageService = imageService;
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
            Role = parentRole,
            CreatedDate = DateTime.UtcNow,
            Status = UserStatus.Active
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

    public async Task<LoginAccountDto> RegisterByPhoneNumberAsync(PhoneNumberRegisterDto dto)
    {
        if (await _unitOfWork.ParentRepository.ExistByPhoneNumberAsync(dto.PhoneNumber))
            throw new ConflictException($"Phone number {dto.PhoneNumber} has been existed.");

        var parentRole = await _unitOfWork.RoleRepository.GetByNameAsync(Constant.ParentRole)
            .ContinueWith(t => t.Result ?? throw new Exception("Role parent name is incorrect."));

        var accountEntity = new Account()
        {
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password),
            FullName = StringUtils.FormatName(dto.FullName),
            Role = parentRole,
            CreatedDate = DateTime.UtcNow,
            Status = UserStatus.Active
        };

        var parentEntity = new Parent()
        {
            PhoneNumber = dto.PhoneNumber,
            Account = accountEntity
        };

        await _unitOfWork.ParentRepository.AddAsync(parentEntity);
        await _unitOfWork.SaveChangeAsync();

        var result = AccountMapper.EntityToLoginAccountDto(accountEntity);
        result.AccessToken = _authenticationService.CreateAccessToken(accountEntity);
        result.RefreshToken = _authenticationService.CreateRefreshToken(accountEntity);
        return result;
    }

    public async Task<LoginAccountDto> LoginByEmailAsync(EmailCredential dto)
    {
        var account = await _unitOfWork.AccountRepository.LoginByEmailAsync(dto.Email)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Account not found."));

        if (!BCrypt.Net.BCrypt.EnhancedVerify(dto.Password, account.PasswordHash))
        {
            throw new UnauthorizedException("Incorrect password.");
        }

        var result = AccountMapper.EntityToLoginAccountDto(account);
        result.AccessToken = _authenticationService.CreateAccessToken(account);
        result.RefreshToken = _authenticationService.CreateRefreshToken(account);
        return result;
    }

    public async Task<LoginAccountDto> LoginByPhoneNumberAsync(PhoneCredential dto)
    {
        var parent = await _unitOfWork.ParentRepository.LoginByPhoneNumberAsync(dto.PhoneNumber)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Account not found."));

        var account = parent.Account;

        if (!BCrypt.Net.BCrypt.EnhancedVerify(dto.Password, account.PasswordHash))
        {
            throw new UnauthorizedException("Incorrect password.");
        }

        var result = AccountMapper.EntityToLoginAccountDto(account);
        result.AccessToken = _authenticationService.CreateAccessToken(account);
        result.RefreshToken = _authenticationService.CreateRefreshToken(account);
        return result;
    }

    public async Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));

        if (!BCrypt.Net.BCrypt.EnhancedVerify(dto.OldPassword, account.PasswordHash))
        {
            throw new BadRequestException("Incorrect password.");
        }

        account.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.NewPassword);

        _unitOfWork.AccountRepository.Update(account);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<string> UpdatePictureAsync(IFormFile file)
    {
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));

        var avatarFileName = $"avatar_{account.Id}";

        var uploadedFile = await _imageService.UploadImage(file, Constant.FirebaseUserAvatarFolder, avatarFileName);

        account.PictureUrl = uploadedFile;

        _unitOfWork.AccountRepository.Update(account);
        await _unitOfWork.SaveChangeAsync();

        return account.PictureUrl;
    }

    public async Task<StudentGameLoginDto> StudentGameLoginAsync(EmailCredential dto)
    {
        var student = await _unitOfWork.StudentRepository.GameStudentLoginAsync(dto.Email)
            .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find student account."));

        var account = student.Account;

        if (!BCrypt.Net.BCrypt.EnhancedVerify(dto.Password, account.PasswordHash))
        {
            throw new UnauthorizedException("Incorrect password.");
        }

        var result = AccountMapper.StudentToStudentGameLoginDto(student);

        result.AccessToken = _authenticationService.CreateAccessToken(student.Account);
        result.RefreshToken = _authenticationService.CreateRefreshToken(student.Account);

        return result;
    }
}