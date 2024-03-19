using Application.Dtos.Response.Account;
using Application.Dtos.Response.Account.Parent;
using Application.Dtos.Response.Account.Student;
using Application.Dtos.Response.Certificate;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Paging;
using Application.Utils;
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

    public static AccountDto AccountToAccountDto(Account entity) => new AccountDto()
    {
        Id = entity.Id,
        Email = entity.Email,
        FullName = entity.FullName,
        PictureUrl = entity.PictureUrl,
        Gender = entity.Gender?.ToString(),
        DateOfBirth = DateUtils.FormatDateTimeToDateV1(entity.DateOfBirth),
        Status = entity.Status.ToString(),
        CreatedDate = DateUtils.FormatDateTimeToDatetimeV1(entity.CreatedDate),
        Role = entity.Role.Name
    };

    public static AdminDto AccountToAdminDto(Account entity) => new AdminDto()
    {
        Id = entity.Id,
        Email = entity.Email,
        FullName = entity.FullName,
        PictureUrl = entity.PictureUrl,
        Gender = entity.Gender?.ToString(),
        DateOfBirth = DateUtils.FormatDateTimeToDateV1(entity.DateOfBirth),
        Status = entity.Status.ToString(),
        CreatedDate = DateUtils.FormatDateTimeToDatetimeV1(entity.CreatedDate),
        Role = entity.Role.Name
    };


    public static StaffDto AccountToStaffDto(Account entity) => new StaffDto()
    {
        Id = entity.Id,
        Email = entity.Email,
        FullName = entity.FullName,
        PictureUrl = entity.PictureUrl,
        Gender = entity.Gender?.ToString(),
        DateOfBirth = DateUtils.FormatDateTimeToDateV1(entity.DateOfBirth),
        Status = entity.Status.ToString(),
        CreatedDate = DateUtils.FormatDateTimeToDatetimeV1(entity.CreatedDate),
        Role = entity.Role.Name,
        Biography = entity.Staff?.Biography,
        PhoneNumber = entity.Staff?.PhoneNumber,
        ProfilePicture = entity.Staff?.ProfilePicture
    };

    public static TeacherDto AccountToTeacherDto(Account entity) => new TeacherDto()
    {
        Id = entity.Id,
        Email = entity.Email,
        FullName = entity.FullName,
        PictureUrl = entity.PictureUrl,
        Gender = entity.Gender?.ToString(),
        DateOfBirth = DateUtils.FormatDateTimeToDateV1(entity.DateOfBirth),
        Status = entity.Status.ToString(),
        CreatedDate = DateUtils.FormatDateTimeToDatetimeV1(entity.CreatedDate),
        Role = entity.Role.Name,
        Field = entity.Teacher?.Field,
        Biography = entity.Teacher?.Biography,
        PhoneNumber = entity.Teacher?.PhoneNumber,
        ProfilePicture = entity.Teacher?.ProfilePicture,
        PersonalInformation = entity.Teacher?.PersonalInformation
    };

    public static ParentResponseDto AccountToParentDto(Account entity) => new ParentResponseDto()
    {
        Id = entity.Id,
        Email = entity.Email,
        FullName = entity.FullName,
        PictureUrl = entity.PictureUrl,
        Gender = entity.Gender?.ToString(),
        DateOfBirth = DateUtils.FormatDateTimeToDateV3(entity.DateOfBirth),
        Status = entity.Status.ToString(),
        CreatedDate = DateUtils.FormatDateTimeToDatetimeV3(entity.CreatedDate),
        Role = entity.Role.Name,
        PhoneNumber = entity.Parent?.PhoneNumber
    };

    public static StudentResponseDto AccountToStudentDto(Account entity) => new StudentResponseDto()
    {
        Id = entity.Id,
        Email = entity.Email,
        FullName = entity.FullName,
        PictureUrl = entity.PictureUrl,
        Gender = entity.Gender?.ToString(),
        DateOfBirth = DateUtils.FormatDateTimeToDateV3(entity.DateOfBirth),
        Status = entity.Status.ToString(),
        CreatedDate = DateUtils.FormatDateTimeToDatetimeV3(entity.CreatedDate),
        Role = entity.Role.Name,
        Age= DateTime.Now.Year -
                (entity.DateOfBirth != null ? entity.DateOfBirth.Value.Year : 0)
    };

    public static PagingResponse<AccountDto> AccountToAccountDto(PagingResponse<Account> entities) =>
        new PagingResponse<AccountDto>()
        {
            TotalPages = entities.TotalPages,
            TotalRecords = entities.TotalRecords,
            Results = entities.Results.Select(AccountToAccountDto).ToList()
        };

    

}