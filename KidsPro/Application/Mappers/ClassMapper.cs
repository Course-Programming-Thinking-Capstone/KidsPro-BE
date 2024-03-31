using Application.Dtos.Response;
using Application.Dtos.Response.Paging;
using Application.Dtos.Response.StudentSchedule;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappers;

public static class ClassMapper
{
    public static ClassCreateResponse ClassToClassCreateResponse(Class dto, string courseName, int slotDuration) =>
        new ClassCreateResponse()
        {
            ClassId = dto.Id,
            ClassCode = dto.Code,
            CourseId = dto.CourseId,
            CourseName = courseName,
            OpenDay = DateUtils.FormatDateTimeToDateV1(dto.OpenDate),
            CloseDay = DateUtils.FormatDateTimeToDateV1(dto.CloseDate),
            Duration = dto.Duration,
            SlotDuration = slotDuration,
            TotalSlot = dto.TotalSlot
        };

    public static ScheduleCreateResponse ScheduleToScheuldeCreateResponse(ClassSchedule dto, List<DayStatus> dayList)
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
            if (dto.Classes!.Count > 0)
            {
                foreach (var c in dto.Classes)
                {
                    var x = new TeacherClass()
                    {
                        ClassId = c.Id,
                        ClassName = c.Code,
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

    public static ClassDetailResponse ClassToClassDetailResponse(Class dto) => new ClassDetailResponse()
    {
        ClassId = dto.Id,
        ClassCode = dto.Code,
        CourseName = dto.Course.Name,
        TeacherId = dto.Teacher?.Id,
        TeacherName = dto.Teacher?.Account.FullName,
        OpenClass = DateUtils.FormatDateTimeToDateV1(dto.OpenDate),
        CloseClass = DateUtils.FormatDateTimeToDateV1(dto.CloseDate),
        Duration = dto.Duration,
        SlotDuration = dto.Course.Syllabus?.SlotTime ?? 0,
        TotalSlot = dto.TotalSlot,
        //Schedules
        RoomUrl = dto.Schedules?.First().RoomUrl,
        SlotNumber = dto.Schedules?.First().Slot ?? 0,
        StartSlot = dto.Schedules?.First().StartTime ?? TimeSpan.Zero,
        EndSlot = dto.Schedules?.First().EndTime ?? TimeSpan.Zero,
        StudyDay = dto.Schedules?.Where(x => x.Status == ScheduleStatus.Active)
            .Select(x => x.StudyDay) ?? new List<DayStatus>(),
        //Students
        Students = dto.Students.Select(x => new StudentClassResponse
        {
            Image = x.Account.PictureUrl,
            StudentName = x.Account.FullName,
            DateOfBirth = DateUtils.FormatDateTimeToDateV1(x.Account.DateOfBirth),
            Gender = x.Account.Gender
        }).ToList(),
        TotalStudent = dto.Students.Count()
    };

    public static ScheduleResponse ScheduleToScheduleResponse(List<ClassSchedule> dto)
        => new ScheduleResponse()
        {
            ClassId = dto.First().ClassId,
            SlotTime = dto.First().Class.Course.Syllabus?.SlotTime,
            StartSlot = dto.First().StartTime,
            EndSlot = dto.First().EndTime,
            SlotNumber = dto.First().Slot,
            RoomUrl = dto.First().RoomUrl,
            StudyDay = dto.Select(x => x.StudyDay),
        };

    public static List<StudentClassResponse> StudentToStudentClassResponse(List<Student> dto)
    {
        return dto.Select(student => new StudentClassResponse
        {
            Image = student.Account.PictureUrl,
            StudentName = student.Account.FullName,
            DateOfBirth = DateUtils.FormatDateTimeToDateV1(student.Account.DateOfBirth),
            Gender = student.Account.Gender
        }).ToList();
    }

    public static PagingClassesResponse ClassToClassesResponse(PagingResponse<Class> dto) => new PagingClassesResponse()
    {
        TotalPage = dto.TotalPages,
        TotalRecord = dto.TotalRecords,
        Classes = dto.Results.Select(c => new ClassesResponse()
        {
            ClassCode = c.Code,
            Start = DateUtils.FormatDateTimeToDateV1(c.OpenDate),
            End = DateUtils.FormatDateTimeToDateV1(c.CloseDate),
            ClassId = c.Id
        }).ToList()
    };

}