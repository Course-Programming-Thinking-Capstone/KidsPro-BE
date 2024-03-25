using Application.Dtos.Response;
using Application.Utils;
using Domain.Entities;

namespace Application.Mappers;

public static class ClassMapper
{
    public static ClassCreateResponse ClassToClassCreateResponse(Class dto, string courseName) =>
        new ClassCreateResponse()
        {
            ClassId = dto.Id,
            ClassCode = dto.Code,
            CourseId = dto.CourseId,
            CourseName = courseName,
            OpenDay = DateUtils.FormatDateTimeToDateV1(dto.OpenDate),
            CloseDay = DateUtils.FormatDateTimeToDateV1(dto.CloseDate),
            Duration = dto.Duration
        };
}