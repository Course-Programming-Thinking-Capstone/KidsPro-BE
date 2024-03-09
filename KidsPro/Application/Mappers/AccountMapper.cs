using Application.Dtos.Response.Account;
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

    public static ParentDto AccountToParentDto(Account entity) => new ParentDto()
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
        PhoneNumber = entity.Parent?.PhoneNumber
    };

    public static StudentDto AccountToStudentDto(Account entity) => new StudentDto()
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

    public static List<StudentDto> ParentToListStudentDto(List<Student> entity)
    {
        var list =new List<StudentDto>();
        foreach (var x in entity)
        {
            var student = new StudentDto();
            student.Id = x.Id;
            student.FullName=x.Account.FullName;
            student.Age =DateTime.Now.Year -
                (x.Account.DateOfBirth!=null?x.Account.DateOfBirth.Value.Year:0);
            list.Add(student);
        }
        return list;
    }

    public static StudentDetailDto ShowStudentDetail(Student entity)
    {
        var student = new StudentDetailDto()
        {
            Id = entity.Id,
            Email = entity.Account.Email,
            FullName = entity.Account.FullName,
            PictureUrl = entity.Account.PictureUrl,
            Gender = entity.Account.Gender?.ToString(),
            DateOfBirth = DateUtils.FormatDateTimeToDateV1(entity.Account.DateOfBirth),
            Status = entity.Account.Status.ToString(),
            CreatedDate = DateUtils.FormatDateTimeToDatetimeV1(entity.Account.CreatedDate),
            Role = entity.Account.Role.Name,
            Age = DateTime.Now.Year -
                (entity.Account.DateOfBirth != null ? entity.Account.DateOfBirth.Value.Year : 0),
            
        };

        if(entity.Certificates != null)
        {
           // student.StudentCourse = new List<(int, string)>();
           // student.StudentCertificate = new List<(string, string)>();
            foreach (var x in entity.Certificates)
            {
                var _course = new TitleDto() { Id = x.Course.Id, Title = x.Course.Name };
                student.StudentCourse.Add(_course);
                var _certificate = new CertificateDto() {title= x.Course.Name,url= x.ResourceUrl };
                student.StudentCertificate.Add(_certificate);
            }
            student.CertificateTotal = entity.Certificates.Count();
        }

        return student;
    }

}