using Application.Dtos.Request.Email;
using Application.Utils;
using Domain.Entities;

namespace Application.Mappers;

public static class EmailMapper
{
    public static EmailContentRequest ShowEmailContentResponse(Student student, string password,Order order)
        => new EmailContentRequest()
        {
            StudentName = student.Account.FullName,
            Birthday = DateUtils.FormatDateTimeToDateV1(student.Account.DateOfBirth),
            Account = student.UserName,
            Password = password,
            Email = order.Parent!.Account.Email,
            ParentId = order.ParentId
        };
}