using Application.Configurations;
using Application.Dtos.Request.Class;
using Application.Dtos.Response;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Class.TeacherClass;
using Application.Dtos.Response.Class.TeacherSchedule;
using Application.Dtos.Response.StudentSchedule;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class ClassService : IClassService
{
    private IUnitOfWork _unitOfWork;
    private IAccountService _account;
    private INotificationService _notify;
    private IDiscordService _discord;
    private IProgressService _progress;

   

    public ClassService(IUnitOfWork unitOfWork, IAccountService account, INotificationService notify,
        IProgressService progress, IDiscordService discord)
    {
        _unitOfWork = unitOfWork;
        _account = account;
        _notify = notify;
        _progress = progress;
        _discord = discord;
    }

    private async Task<AccountDto> CheckPermission()
    {
        var account = await _account.GetCurrentAccountInformationAsync();

        if (account.Role != Constant.StaffRole && account.Role != Constant.AdminRole)
            throw new UnauthorizedException("Not the staff or admin role, please login by manager account");
        return account;
    }

    

    #region Class

    public async Task UpdateClassStatusAsync(int classId, ClassStatus status)
    {
        var entityClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId)
            ?? throw new NotFoundException($"ClassId {classId} not found");
        switch (status)
        {
            case ClassStatus.OnGoing:
                if (entityClass.Schedules!.Count == 0 || entityClass.TeacherId == null)
                    throw new BadRequestException("Update status failed because class doesn't has teacher or schedule");
                break;
        }
        entityClass.Status = status;
        _unitOfWork.ClassRepository.Update(entityClass);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<TeacherClassDto> GetTeacherClassByCodeAsync(string classCode)
    {
        var entity = await _unitOfWork.ClassRepository.GetClassByCodeAsync(classCode)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Class {classCode} not found."));
        return ClassMapper.ClassToTeacherClassDto(entity);
    }

    public async Task<ClassCreateResponse> CreateClassAsync(ClassCreateRequest dto)
    {
        var account = await CheckPermission();

        if (await _unitOfWork.ClassRepository.ExistByClassCodeAsync(dto.ClassCode))
            throw new BadRequestException("Class Code has been existed");

        var course = await _unitOfWork.CourseRepository.GetByIdAsync(dto.CourseId) ??
                     throw new BadRequestException("Course Id not exist");

        var classEntity = new Class()
        {
            Code = dto.ClassCode,
            OpenDate = dto.OpenDay,
            CloseDate = dto.CloseDay,
            Status = ClassStatus.Opening,
            CourseId = dto.CourseId,
            CreatedById = account.Id,
            Duration = dto.CloseDay.Month - dto.OpenDay.Month,
            TotalSlot = course.Syllabus?.TotalSlot ?? 0
        };

        await _unitOfWork.ClassRepository.AddAsync(classEntity);
        await _unitOfWork.SaveChangeAsync();

        return ClassMapper.ClassToClassCreateResponse(classEntity, course.Name, course.Syllabus?.SlotTime ?? 0);
    }

    public async Task<ClassDetailResponse> GetClassByIdAsync(int classId)
    {
        var account = await _account.GetCurrentAccountInformationAsync();

        var entityClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);

        return entityClass != null
            ? ClassMapper.ClassToClassDetailResponse(entityClass)
            : throw new BadRequestException($"ClassId: {classId} doesn't exist");
    }

    public async Task<PagingClassesResponse> GetClassesAsync(int? page, int? size)
    {
        //set default page size
        if (!page.HasValue || !size.HasValue)
        {
            page = 1;
            size = 10;
        }

        var classes =
            await _unitOfWork.ClassRepository.GetPaginateAsync(filter: null, orderBy: null, page: page, size: size);

        return ClassMapper.ClassToClassesPagingResponse(classes);
    }

    public async Task<List<ClassesResponse>> GetClassByRoleAsync()
    {
        var account = await _account.GetCurrentAccountInformationAsync();

        var classes = await _unitOfWork.ClassRepository.GetClassByRoleAsync(account.IdSubRole, account.Role);
        if (classes.Count == 0) throw new NotFoundException($"Teacher or student do not have a class");

        var classResponse= ClassMapper.ClassToClassesResponse(classes);

        foreach (var x in classResponse)
        {
            var course = await _progress.GetCourseProgressAsync(account.IdSubRole, x.CourseId);
            x.CourseProgress =course?.CourseProgress??0;
        }

        return classResponse;
    }

    #endregion

    #region Schedule

    public async Task<ScheduleCreateResponse> CreateScheduleAsync(ScheduleCreateRequest dto)
    {
        await CheckPermission();

        var entityClass = await _unitOfWork.ClassRepository.GetByIdAsync(dto.ClassId)
                          ?? throw new BadRequestException($"ClassID {dto.ClassId} not found");

        string? discordLink = null;
        if (dto.RoomUrl == string.Empty)
            discordLink = await _discord.CreateVoiceChannelAsync("Class: " + entityClass.Code);

        var time = TimeUtils.GetTimeFromSlot(dto.Slot, dto.SlotTime);

        var schedule = dto.Days.Select(day => new ClassSchedule
        {
            RoomUrl = discordLink ?? dto.RoomUrl,
            ClassId = dto.ClassId,
            Slot = dto.Slot,
            StudyDay = day,
            StartTime = time.Item1,
            EndTime = time.Item2
        }).ToList();

        await _unitOfWork.ScheduleReposisoty.AddRangeAsync(schedule);
        await _unitOfWork.SaveChangeAsync();

        return ClassMapper.ScheduleToScheduleCreateResponse(schedule.First(), dto.Days);
    }

    public async Task<ScheduleResponse> GetScheduleByClassIdAsync(int classId)
    {
        await CheckPermission();

        var schedule = await _unitOfWork.ScheduleReposisoty.GetScheduleByClassIdAsync(classId, ScheduleStatus.Active);

        return schedule.Any()
            ? ClassMapper.ScheduleToScheduleResponse(schedule)
            : throw new BadRequestException($"ClassId: {classId} doesn't exist");
    }

    public async Task UpdateScheduleAsync(ScheduleUpdateRequest dto)
    {
        await CheckPermission();

        var schedules =
            await _unitOfWork.ScheduleReposisoty.GetScheduleByClassIdAsync(dto.ClassId, ScheduleStatus.AllStatus);

        if (schedules.Any())
        {
            var studyDays = dto.StudyDay
                            ?? throw new BadRequestException("Day list is null");

            var time = TimeUtils.GetTimeFromSlot(dto.SlotNumber, dto.SlotTime);

            //Update Schedules
            foreach (var (schedule, studyDay) in schedules.Zip(studyDays))
            {
                schedule.StudyDay = studyDay;
                schedule.StartTime = time.Item1;
                schedule.EndTime = time.Item2;
                schedule.Slot = dto.SlotNumber;
                schedule.Status = ScheduleStatus.Active;
            }

            //Nếu truước đó có nhiều schedules mà giồ click bỏ bớt
            if (schedules.Count() > studyDays.Count())
            {
                var remainingSchedule
                    = schedules.Skip(studyDays.Count()).ToList();
                foreach (var x in remainingSchedule)
                {
                    x.Status = ScheduleStatus.Inactive;
                }
            }

            _unitOfWork.ScheduleReposisoty.UpdateRange(schedules);
            await _unitOfWork.SaveChangeAsync();

            //Add thêm schedule nếu ở hàm update có chọn thêm slot học
            if (schedules.Count() < studyDays.Count())
            {
                var remainingStudyDays
                    = studyDays.Skip(schedules.Count()).ToList();
                var entity = new ScheduleCreateRequest()
                {
                    ClassId = dto.ClassId,
                    Slot = dto.SlotNumber,
                    RoomUrl = schedules.FirstOrDefault()?.RoomUrl,
                    Days = remainingStudyDays,
                    SlotTime = dto.SlotTime
                };
                await CreateScheduleAsync(entity);
            }
        }
    }

    #endregion

    #region Teacher

    public async Task<string> AddTeacherToClassAsync(int teacherId, int classId)
    {
        await CheckPermission();

        var entityClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId)
                          ?? throw new NotFoundException($"ClassId: {classId} doesn't exist");

        var teacher = await _unitOfWork.TeacherRepository.GetTeacherSchedulesById(teacherId)
                      ?? throw new NotFoundException($"TeacherId: {teacherId} doesn't exist");

        bool hasOverlap = entityClass.Schedules!
            .Any(c => teacher.Classes!
                .Any(t => t.Schedules!
                    .Any(s => s.Slot == c.Slot && s.StudyDay == c.StudyDay)));

        if (hasOverlap) throw new BadRequestException("Teachers have conflicting teaching schedules");

        entityClass.TeacherId = teacherId;
        _unitOfWork.ClassRepository.Update(entityClass);
        await _unitOfWork.SaveChangeAsync();

        //Sent notice to teacher
        var title = "New class";
        var content = "Your new class is " + entityClass.Code + ", click on My Classes for more details";
        await _notify.SendNotifyToAccountAsync(teacherId, title, content);

        return teacher.Account.FullName;
    }

    public async Task<List<TeacherScheduleResponse>> GetTeacherToClassAsync()
    {
        await CheckPermission();
        var teachers = await _unitOfWork.TeacherRepository.GetTeacherSchedules();
        return ClassMapper.TeacherToTeacherScheduleResponse(teachers);
    }

    #endregion

    #region Student

    public async Task<List<StudentClassResponse>> SearchStudentScheduleAsync(string input, int classId)
    {
        await CheckPermission();

        var students = await _unitOfWork.StudentRepository.SearchStudent(input, SearchType.ClassStudent);

        var entityClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId)
                          ?? throw new NotFoundException($"ClassId: {classId} doesn't exist");

        var studentsForClass = GetStudentsCanAddToClass(students, entityClass);

        return ClassMapper.StudentToStudentClassResponse(studentsForClass);
    }

    public List<Student> GetStudentsCanAddToClass(List<Student> students, Class entityClass)
    {
        return students.Where(c => !c.Classes.Any() || c.Classes
            .All(s => s.Schedules!.All(x => entityClass.Schedules!
                .All(e => e.StudyDay != x.StudyDay && e.Slot != x.Slot)))).ToList();
    }

    public async Task<List<StudentClassResponse>> UpdateStudentsToClassAsync(StudentsAddRequest dto)
    {
        var entityClass = await _unitOfWork.ClassRepository.GetByIdAsync(dto.ClassId)
                          ?? throw new NotFoundException($"ClassId: {dto.ClassId} doesn't exist");

        var students = await _unitOfWork.StudentRepository.GetStudentsByIds(dto.StudentIds);

        if (students.Count < dto.StudentIds.Count)
            throw new BadRequestException("StudentId doesn't exist");

        if (entityClass.Students.Count == 0)
            entityClass.Students = new List<Student>();

        // lấy những StudentId mà list truyền vào có, list không có để add
        var addStudents = students.Where(e => entityClass.Students.All(s => s.Id != e.Id)).ToList();
        foreach (var x in addStudents)
            entityClass.Students.Add(x);

        // lấy những StudentId mà class có, list truyền vào không có để removed
        var removeStudents = entityClass.Students.Where(e => students.All(s => s.Id != e.Id)).ToList();
        foreach (var x in removeStudents)
            entityClass.Students.Remove(x);

        _unitOfWork.ClassRepository.Update(entityClass);
        await _unitOfWork.SaveChangeAsync();

        return ClassMapper.StudentToStudentClassResponse(entityClass.Students.ToList());
    }

    #endregion
}