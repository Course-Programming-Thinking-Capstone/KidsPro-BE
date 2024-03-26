using Application.Dtos.Response;
using Application.Dtos.Response.StudentSchedule;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappers;

public static class ClassMapper
{
    public static ClassCreateResponse ClassToClassCreateResponse(Class dto, string courseName, int slotTime) =>
        new ClassCreateResponse()
        {
            ClassId = dto.Id,
            ClassCode = dto.Code,
            CourseId = dto.CourseId,
            CourseName = courseName,
            OpenDay = DateUtils.FormatDateTimeToDateV1(dto.OpenDate),
            CloseDay = DateUtils.FormatDateTimeToDateV1(dto.CloseDate),
            Duration = dto.Duration,
            SlotTime = slotTime,
            TotalSlot = dto.TotalSlot
        };

    public static ScheduleCreateResponse ScheduleToScheuldeCreateResponse(ClassSchedule dto, List<int> dayList)
        => new ScheduleCreateResponse()
        {
            ClassId = dto.ClassId,
            Days = dayList,
            Slot = dto.Slot,
            Open = dto.StartTime,
            Close = dto.EndTime,
            Link = dto.RoomUrl
        };

    public static List<TeacherScheduleResponse> TeacherToTeacherScheduleResponse(List<Teacher> teachers)
    {
        var teacherScheduleList = new List<TeacherScheduleResponse>();

        foreach (var dto in teachers)
        {
            var teacher = new TeacherScheduleResponse()
            {
                TeacherId = dto.Id,
                TeacherName = dto.Account.FullName,
            };
            //Nêú teacher có class, sẽ add schedule vào
            if (dto.Classes != null)
            {
                foreach (var c in dto.Classes)
                {
                    var x = new TeacherCouse()
                    {
                        CourseId = c.CourseId,
                        CourseName = c.Course.Name,
                        Open = c.Schedules!.First().StartTime,
                        Close = c.Schedules!.First().EndTime,
                        Slot = c.Schedules!.First().Slot,
                        StudyDays = c.Schedules!.Select(x => x.StudyDay),
                    };
                    teacher.Schedules?.Add(x);
                }
            }

            teacherScheduleList.Add(teacher);
        }

        return teacherScheduleList;
    }

    public static ClassResponse ClassToClassResponse(Class dto) => new ClassResponse()
    {
        ClassId = dto.Id,
        ClassCode = dto.Code,
        CourseName = dto.Course.Name,
        TeacherId = dto.Teacher?.Id,
        TeacherName = dto.Teacher?.Account.FullName,
                      //?? "The class doesn't have a teacher yet",
        OpenClass = DateUtils.FormatDateTimeToDateV1(dto.OpenDate),
        CloseClass = DateUtils.FormatDateTimeToDateV1(dto.CloseDate),
        Duration = dto.Duration,
        SlotTime = dto.Course.Syllabus?.SlotTime ?? 0,
        TotalSlot = dto.TotalSlot,
        RoomUrl = dto.Schedules?.First().RoomUrl,
        //?? "The Class doesn't have a schedule yet"
        SlotNumber = dto.Schedules?.First().Slot ?? 0,
        StartSlot = dto.Schedules?.First().StartTime ?? TimeSpan.Zero,
        EndSlot = dto.Schedules?.First().EndTime ?? TimeSpan.Zero,
        StudyDay = dto.Schedules?.Select(x => x.StudyDay) ?? new List<DayStatus>(),
        Students = dto.Students.Select(x => new StudentClassResponse
        {
            Image = x.Account.PictureUrl,
            StudentName = x.Account.FullName,
            Age = Math.Max(0, DateTime.Now.Year - (x.Account.DateOfBirth?.Year ?? 0)),
            Gender = x.Account.Gender
        }).ToList(),
        TotalStudent = dto.Students.Count()
    };
}