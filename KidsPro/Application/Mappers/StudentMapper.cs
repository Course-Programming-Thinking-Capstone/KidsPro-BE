using Application.Dtos.Response.Account.Student;
using Application.Dtos.Response.Certificate;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.StudentProgress;
using Application.Utils;
using Domain.Entities;

namespace Application.Mappers;

public static class StudentMapper
{
    public static List<StudentResponse> ShowStudentList(List<Student> entity)
    {
        var list = new List<StudentResponse>();
        foreach (var x in entity)
        {
            var student = new StudentResponse();
            student.Id = x.Id;
            student.FullName = x.Account.FullName;
            student.Age = Math.Max(0, DateTime.Now.Year - (x.Account.DateOfBirth?.Year ?? 0));
            student.Gender = x.Account.Gender?.ToString();
            student.ParentId = x.ParentId;
            student.ParentName = x.Parent.Account.FullName;
            student.DateOfBirth = DateUtils.FormatDateTimeToDateV3(x.Account.DateOfBirth);
            list.Add(student);
        }

        return list;
    }

    public static StudentDetailResponse ShowStudentDetail(Student entity, List<SectionProgressResponse>? progress)
    {
        var student = new StudentDetailResponse()
        {
            UserName = entity.UserName,
            Id = entity.Id,
            Email = entity.Account.Email,
            FullName = entity.Account.FullName,
            PictureUrl = entity.Account.PictureUrl,
            Gender = entity.Account.Gender?.ToString(),
            DateOfBirth = DateUtils.FormatDateTimeToDateV3(entity.Account.DateOfBirth),
            Status = entity.Account.Status.ToString(),
            CreatedDate = DateUtils.FormatDateTimeToDatetimeV3(entity.Account.CreatedDate),
            Role = entity.Account.Role.Name,
            Age = Math.Max(0, DateTime.Now.Year - (entity.Account.DateOfBirth?.Year ?? 0)),
            ParentId = entity.ParentId,
            ParentName = entity.Parent.Account.FullName,
            StudentsCertificate = entity.Certificates?.Select(x => new CertificateDto
            {
                CourseName = x.Course.Name,
                CertificateUrl = x.ResourceUrl
            }).ToList(),
            CertificateTotal = entity.Certificates?.Count ?? 0,
            StudentsCourse = entity.Classes.Select(x => new StudentCoursesDto
            {
                CourseId = x.CourseId,
                CourseName = x.Course.Name,
                CourseProgress = progress?.FirstOrDefault(z => z.CourseId == x.CourseId)?.CourseProgress,
            }).ToList(),
            CourseTotal = entity.Classes?.Count ?? 0,
        };

        return student;
    }
}