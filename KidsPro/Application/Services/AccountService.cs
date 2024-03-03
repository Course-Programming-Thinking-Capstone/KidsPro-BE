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
using Microsoft.Extensions.Logging;
using Constant = Application.Configurations.Constant;

namespace Application.Services;

public class AccountService : IAccountService
{
    private IUnitOfWork _unitOfWork;
    private IAuthenticationService _authenticationService;
    private IImageService _imageService;
    private ILogger<AccountService> _logger;


    public AccountService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService,
        IImageService imageService, ILogger<AccountService> logger)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
        _imageService = imageService;
        _logger = logger;
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

    public async Task<AccountDto> GetCurrentAccountInformationAsync()
    {
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        Account account;
        AccountDto result;
        switch (role)
        {
            case Constant.ParentRole:
            {
                account = await _unitOfWork.AccountRepository.GetParentAccountById(accountId)
                    .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));
                result = AccountMapper.AccountToParentDto(account);
                break;
            }

            case Constant.AdminRole:
            {
                account = await _unitOfWork.AccountRepository.GetAdminAccountById(accountId)
                    .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));
                result = AccountMapper.AccountToAdminDto(account);
                break;
            }

            case Constant.StaffRole:
            {
                account = await _unitOfWork.AccountRepository.GetStaffAccountById(accountId)
                    .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));
                result = AccountMapper.AccountToStaffDto(account);
                break;
            }

            case Constant.TeacherRole:
            {
                account = await _unitOfWork.AccountRepository.GetTeacherAccountById(accountId)
                    .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));
                result = AccountMapper.AccountToTeacherDto(account);
                break;
            }

            case Constant.StudentRole:
            {
                account = await _unitOfWork.AccountRepository.GetStudentAccountById(accountId)
                    .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));
                result = AccountMapper.AccountToStudentDto(account);
                break;
            }

            default: throw new UnauthorizedException("Invalid token.");
        }

        return result;
    }

    public async Task<AccountDto> GetAccountByIdAsync(int id, string role)
    {
        Account account;
        AccountDto result;
        switch (role)
        {
            case Constant.ParentRole:
            {
                account = await _unitOfWork.AccountRepository.GetParentAccountById(id)
                    .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));
                result = AccountMapper.AccountToParentDto(account);
                break;
            }

            case Constant.AdminRole:
            {
                account = await _unitOfWork.AccountRepository.GetAdminAccountById(id)
                    .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));
                result = AccountMapper.AccountToAdminDto(account);
                break;
            }

            case Constant.StaffRole:
            {
                account = await _unitOfWork.AccountRepository.GetStaffAccountById(id)
                    .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));
                result = AccountMapper.AccountToStaffDto(account);
                break;
            }

            case Constant.TeacherRole:
            {
                account = await _unitOfWork.AccountRepository.GetTeacherAccountById(id)
                    .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));
                result = AccountMapper.AccountToTeacherDto(account);
                break;
            }

            case Constant.StudentRole:
            {
                account = await _unitOfWork.AccountRepository.GetStudentAccountById(id)
                    .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));
                result = AccountMapper.AccountToStudentDto(account);
                break;
            }

            default: throw new UnauthorizedException("Invalid role.");
        }

        return result;
    }

    public async Task<AccountDto> CreateAccountAsync(CreateAccountDto dto)
    {
        if (await _unitOfWork.AccountRepository.ExistByEmailAsync(dto.Email))
            throw new ConflictException($"Email {dto.Email} has been existed.");

        var account = new Account()
        {
            Email = dto.Email,
            FullName = StringUtils.FormatName(dto.FullName),
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("0000"),
            Status = UserStatus.Active,
            IsDelete = false,
            CreatedDate = DateTime.UtcNow
        };

        AccountDto result;

        switch (dto.Role)
        {
            case Constant.StaffRole:
            {
                var staffRole = await _unitOfWork.RoleRepository.GetByNameAsync(Constant.StaffRole)
                    .ContinueWith(t => t.Result ?? throw new Exception("Role staff name is incorrect."));

                account.Role = staffRole;
                var staff = new Staff()
                {
                    PhoneNumber = dto.PhoneNumber,
                    Account = account
                };

                await _unitOfWork.StaffRepository.AddAsync(staff);
                await _unitOfWork.SaveChangeAsync();
                result = AccountMapper.AccountToAccountDto(staff.Account);

                break;
            }

            case Constant.TeacherRole:
            {
                var teacherRole = await _unitOfWork.RoleRepository.GetByNameAsync(Constant.TeacherRole)
                    .ContinueWith(t => t.Result ?? throw new Exception("Role teacher name is incorrect."));

                account.Role = teacherRole;
                var teacher = new Teacher()
                {
                    PhoneNumber = dto.PhoneNumber,
                    Account = account
                };

                await _unitOfWork.TeacherRepository.AddAsync(teacher);
                await _unitOfWork.SaveChangeAsync();

                result = AccountMapper.AccountToAccountDto(teacher.Account);
                break;
            }

            default:
                throw new UnauthorizedException($"Only accept role {Constant.StaffRole} and {Constant.TeacherRole}");
        }

        return result;
    }
}