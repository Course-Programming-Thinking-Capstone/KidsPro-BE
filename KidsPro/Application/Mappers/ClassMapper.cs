using Application.Dtos.Response;
using Application.Dtos.Response.Class.TeacherClass;
using Application.Dtos.Response.Class.TeacherSchedule;
using Application.Dtos.Response.Paging;
using Application.Dtos.Response.StudentSchedule;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappers;

public static class ClassMapper
{
    public static ClassCreateResponse ClassToClassCreateResponse(Class dto, string courseName, Syllabus? syllabus) =>
        new ClassCreateResponse()
        {
            ClassId = dto.Id,
            ClassCode = dto.Code,
            CourseId = dto.CourseId,
            CourseName = courseName,
            OpenDay = DateUtils.FormatDateTimeToDateV3(dto.OpenDate),
            CloseDay = DateUtils.FormatDateTimeToDateV3(dto.CloseDate),
            DayOfWeekStart = DateUtils.FormatDateTimeToDayOfWeek(dto.OpenDate),
            DayOfWeekEnd = DateUtils.FormatDateTimeToDayOfWeek(dto.CloseDate),
            ClassDuration = dto.Duration,
            SlotDuration = syllabus?.SlotTime,
            SlotPerWeek = syllabus?.SlotPerWeek,
            TotalSlot = dto.TotalSlot
        };

    public static ScheduleCreateResponse ScheduleToScheduleCreateResponse(ClassSchedule dto, List<DayStatus> dayList)
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
                        Open = c.Schedules?.FirstOrDefault()?.StartTime,
                        Close = c.Schedules?.FirstOrDefault()?.EndTime,
                        Slot = c.Schedules?.FirstOrDefault()?.Slot,
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
        ClassStatus = dto.Status,
        OpenClass = DateUtils.FormatDateTimeToDateV3(dto.OpenDate),
        CloseClass = DateUtils.FormatDateTimeToDateV3(dto.CloseDate),
        DayOfWeekStart = DateUtils.FormatDateTimeToDayOfWeek(dto.OpenDate),
        DayOfWeekEnd = DateUtils.FormatDateTimeToDayOfWeek(dto.CloseDate),
        Duration = dto.Duration,
        SlotDuration = dto.Course.Syllabus?.SlotTime ?? 0,
        TotalSlot = dto.TotalSlot,
        SlotPerWeek = dto.Course.Syllabus?.SlotPerWeek ?? 0,
        //Teacher
        TeacherId = dto.Teacher?.Id,
        TeacherName = dto.Teacher?.Account.FullName,
        TeacherPhoneNumber = dto.Teacher?.PhoneNumber,
        TeacherEmail = dto.Teacher?.Account.Email,
        //Schedules
        RoomUrl = dto.Schedules?.FirstOrDefault()?.RoomUrl,
        SlotNumber = dto.Schedules?.FirstOrDefault()?.Slot ?? 0,
        StartSlot = dto.Schedules?.FirstOrDefault()?.StartTime ?? TimeSpan.Zero,
        EndSlot = dto.Schedules?.FirstOrDefault()?.EndTime ?? TimeSpan.Zero,
        StudyDay = dto.Schedules?.Where(x => x.Status == ScheduleStatus.Active)
            .Select(x => x.StudyDay) ?? new List<DayStatus>(),
        //Students
        Students = dto.Students.Select(x => new StudentClassResponse
        {
            StudentId = x.Id,
            StudentName = x.Account.FullName,
            DateOfBirth = DateUtils.FormatDateTimeToDateV3(x.Account.DateOfBirth),
            Gender = x.Account.Gender
        }).ToList(),
        TotalStudent = dto.Students.Count()
    };

    public static ScheduleResponse ScheduleToScheduleResponse(List<ClassSchedule> dto)
        => new ScheduleResponse()
        {
            ClassId = dto.FirstOrDefault()?.ClassId,
            SlotPerWeek = dto.FirstOrDefault()?.Class.Course.Syllabus?.SlotPerWeek,
            SlotTime = dto.FirstOrDefault()?.Class.Course.Syllabus?.SlotTime,
            StartSlot = dto.FirstOrDefault()?.StartTime,
            EndSlot = dto.FirstOrDefault()?.EndTime,
            SlotNumber = dto.FirstOrDefault()?.Slot,
            RoomUrl = dto.FirstOrDefault()?.RoomUrl,
            StudyDay = dto.Select(x => x.StudyDay),
        };

    public static List<StudentClassResponse> StudentToStudentClassResponse(List<Student> dto)
    {
        return dto.Select(student => new StudentClassResponse
        {
            StudentId = student.Id,
            Image = student.Account.PictureUrl,
            StudentName = student.Account.FullName,
            DateOfBirth = DateUtils.FormatDateTimeToDateV3(student.Account.DateOfBirth),
            Gender = student.Account.Gender
        }).ToList();
    }

    public static PagingClassesResponse ClassToClassesPagingResponse(PagingResponse<Class> dto) =>
        new PagingClassesResponse()
        {
            TotalPage = dto.TotalPages,
            TotalRecord = dto.TotalRecords,
            Classes = dto.Results.Select(c => new ClassesResponse()
            {
                ClassCode = c.Code,
                OpenClass = DateUtils.FormatDateTimeToDateV3(c.OpenDate),
                CloseClass = DateUtils.FormatDateTimeToDateV3(c.CloseDate),
                DayOfWeekStart = DateUtils.FormatDateTimeToDayOfWeek(c.OpenDate),
                DayOfWeekEnd = DateUtils.FormatDateTimeToDayOfWeek(c.CloseDate),
                ClassId = c.Id,
                ClassStatus = c.Status
            }).ToList()
        };

    public static List<ClassesResponse> ClassToClassesResponse(List<Class> dto)
    {
        var result = new List<ClassesResponse>();
        foreach (var x in dto)
        {
            if (x.Status == ClassStatus.OnGoing)
            {
                var entityClass = new ClassesResponse
                {
                    ClassId = x.Id,
                    ClassCode = x.Code,
                    StartSlot = x.Schedules?.FirstOrDefault()?.StartTime.ToString(),
                    EndSlot = x.Schedules?.FirstOrDefault()?.EndTime.ToString(),
                    OpenClass = DateUtils.FormatDateTimeToDateV3(x.OpenDate),
                    CloseClass = DateUtils.FormatDateTimeToDateV3(x.CloseDate),
                    DayOfWeekStart = DateUtils.FormatDateTimeToDayOfWeek(x.OpenDate),
                    DayOfWeekEnd = DateUtils.FormatDateTimeToDayOfWeek(x.CloseDate),
                    Days = x.Schedules?.Select(s => s.StudyDay).ToList(),
                    TeacherName = x.Teacher?.Account.FullName,
                    CourseName = x.Course.Name,
                    CourseId = x.CourseId,
                    CourseImage = x.Course.PictureUrl,
                    ClassStatus = x.Status,
                    RoomUrl = x.Schedules?.FirstOrDefault()?.RoomUrl,
                    Duration = x.Duration,
                    SlotDuration = x.Course.Syllabus?.SlotTime ?? 0,
                };
                result.Add(entityClass);
            }
        }

        return result;
    }

    public static TeacherClassStudentDto StudentToTeacherClassStudentDto(Student entity)
    {
        return new TeacherClassStudentDto()
        {
            AccountId = entity.AccountId,
            FullName = entity.Account.FullName,
            PictureUrl = entity.Account.PictureUrl,
            Gender = entity.Account.Gender?.ToString() ?? "Other",
            Age = DateUtils.CalculateAge(entity.Account.DateOfBirth),
        };
    }

    public static TeacherClassDto ClassToTeacherClassDto(Class entity)
    {
        return new TeacherClassDto()
        {
            Id = entity.Id,
            Code = entity.Code,
            TotalSlot = entity.TotalSlot,
            Status = entity.Status.ToString(),
            Duration = entity.Duration,
            CourseName = entity.Course.Name,
            CourseId = entity.CourseId,
            TotalStudent = entity.TotalStudent,
            Students = entity.Students.Select(StudentToTeacherClassStudentDto).ToList()
        };
    }
}