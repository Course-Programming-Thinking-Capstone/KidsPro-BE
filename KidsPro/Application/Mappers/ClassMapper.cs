using Application.Dtos.Response;
using Application.Utils;
using Domain.Entities;

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

    // public static ClassResponse ClassToClassResponse(Class dto)
    // {
    //     var entityClass= new ClassResponse()
    //     {
    //         ClassId = dto.Id,
    //         ClassCode = dto.Code,
    //         CourseName = dto.Course.Name,
    //         TeacherName = dto.Teacher?.Account.FullName??"The class doesn't have a teacher yet",
    //         
    //     }
    // }
}