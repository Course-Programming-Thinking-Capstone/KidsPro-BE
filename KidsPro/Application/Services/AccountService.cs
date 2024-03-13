using System.Linq.Expressions;
using Application.Dtos.Request.Authentication;
using Application.Dtos.Request.User;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Paging;
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

    public async Task<PagingResponse<AccountDto>> FilterAccountAsync(string? fullName, Gender? gender, string? status,
        string? sortFullName, string? sortCreatedDate, int? page,
        int? size)
    {
        //need to check role
        var parameter = Expression.Parameter(typeof(Account));
        Expression filter = Expression.Constant(true); // default is "where true"

        //set default page size
        if (!page.HasValue || !size.HasValue)
        {
            page = 1;
            size = 10;
        }

        try
        {
            //get account that is not deleted
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(Account.IsDelete)),
                    Expression.Constant(false)));

            //Get account that is not admin
            filter = Expression.AndAlso(filter,
                Expression.NotEqual(Expression.Property(parameter, nameof(Account.RoleId)),
                    Expression.Constant(1)));

            if (!string.IsNullOrEmpty(fullName))
            {
                filter = Expression.AndAlso(filter,
                    Expression.Call(
                        Expression.Property(parameter, nameof(Account.FullName)),
                        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })
                        ?? throw new NotImplementException(
                            $"{nameof(string.Contains)} method is deprecated or not supported."),
                        Expression.Constant(fullName)));
            }

            if (gender.HasValue)
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Account.Gender)),
                        Expression.Constant(gender)));
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse(status, true, out UserStatus statusEnum))
                {
                    filter = Expression.AndAlso(filter,
                        Expression.Equal(Expression.Property(parameter, nameof(Account.Status)),
                            Expression.Constant(statusEnum)));
                }
                else
                {
                    throw new BadRequestException($"Invalid Status value: {status}");
                }
            }

            Func<IQueryable<Account>, IOrderedQueryable<Account>> orderBy = q => q.OrderBy(a => a.Id);

            if (sortFullName != null && sortFullName.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.FullName);
            }
            else if (sortFullName != null && sortFullName.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.FullName);
            }
            else if (sortCreatedDate != null && sortCreatedDate.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.CreatedDate);
            }
            else if (sortCreatedDate != null && sortCreatedDate.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.CreatedDate);
            }

            var entities = await _unitOfWork.AccountRepository.GetPaginateAsync(
                filter: Expression.Lambda<Func<Account, bool>>(filter, parameter),
                orderBy: orderBy,
                page: page,
                size: size,
                includeProperties: $"{nameof(Account.Role)}"
            );
            var result = AccountMapper.AccountToAccountDto(entities);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when execute {methodName} method: \nDetail: {errorDetail}.",
                nameof(this.FilterAccountAsync), e.Message);
            throw new Exception($"Error when execute {nameof(this.FilterAccountAsync)} method");
        }
    }

   


}