using Application.Configurations;
using Application.Dtos.Request.Class;
using Application.Dtos.Response;
using Application.Dtos.Response.Account;
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

    public ClassService(IUnitOfWork unitOfWork, IAccountService account, INotificationService notify)
    {
        _unitOfWork = unitOfWork;
        _account = account;
        _notify = notify;
    }

    private async Task<AccountDto> CheckPermission()
    {
        var account = await _account.GetCurrentAccountInformationAsync();

        if (account.Role != Constant.StaffRole && account.Role != Constant.AdminRole)
            throw new UnauthorizedException("Not the staff role, please login by staff account");
        return account;
    }

    public async Task<ClassCreateResponse> CreateClassAsync(ClassCreateRequest dto)
    {
        var account = await CheckPermission();

        if (await _unitOfWork.ClassRepository.ExistByClassCode(dto.ClassCode))
            throw new BadRequestException("Class Code has been existed");

        var course = await _unitOfWork.CourseRepository.GetByIdAsync(dto.CourseId) ??
                     throw new BadRequestException("Course Id not exist");

        var classEntity = new Class()
        {
            Code = dto.ClassCode,
            OpenDate = dto.OpenDay,
            CloseDate = dto.CloseDay,
            Status = ClassStatus.Active,
            CourseId = dto.CourseId,
            CreatedById = account.Id,
            Duration = dto.CloseDay.Month - dto.OpenDay.Month,
            TotalSlot = course.Syllabus?.TotalSlot ?? 0
        };

        await _unitOfWork.ClassRepository.AddAsync(classEntity);
        await _unitOfWork.SaveChangeAsync();

        return ClassMapper.ClassToClassCreateResponse(classEntity, course.Name, course.Syllabus?.SlotTime ?? 0);
    }

    public async Task<ScheduleCreateResponse> CreateScheduleAsync(ScheduleCreateRequest dto)
    {
        await CheckPermission();

        var time = TimeUtils.GetTimeFromSlot(dto.Slot, dto.SlotTime);

        var schedule = dto.Days.Select(day => new ClassSchedule
        {
            RoomUrl = dto.RoomUrl,
            ClassId = dto.ClassId,
            Slot = dto.Slot,
            StudyDay = day,
            StartTime = time.Item1,
            EndTime = time.Item2
        }).ToList();

        await _unitOfWork.ScheduleReposisoty.AddRangeAsync(schedule);
        await _unitOfWork.SaveChangeAsync();

        return ClassMapper.ScheduleToScheuldeCreateResponse(schedule.First(), dto.Days);
    }

    public async Task<string> AddTeacherToClassAsync(int teacherId, int classId)
    {
        await CheckPermission();

        var entityClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId)
                          ?? throw new NotFoundException($"ClassId: {classId} doesn't exist");

        entityClass.TeacherId = teacherId;

        _unitOfWork.ClassRepository.Update(entityClass);
        await _unitOfWork.SaveChangeAsync();

        var teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(teacherId);
        return teacher?.Account.FullName ?? "";
    }

    public async Task<List<TeacherScheduleResponse>> GetTeacherToClassAsync()
    {
        await CheckPermission();
        var teachers = await _unitOfWork.TeacherRepository.GetTeacherToClass();
        return ClassMapper.TeacherToTeacherScheduleResponse(teachers);
    }

    public async Task<ClassResponse> GetClassByIdAsync(int classId)
    {
        await CheckPermission();
        var entityClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);

        return entityClass != null
            ? ClassMapper.ClassToClassResponse(entityClass)
            : throw new BadRequestException($"ClassId: {classId} doesn't exist");
    }

    public async Task<ScheduleResponse> GetScheduleByClassIdAsync(int classId)
    {
        await CheckPermission();

        var schedule = await _unitOfWork.ScheduleReposisoty.GetScheduleByClassIdAsync(classId,ScheduleStatus.Active);

        return schedule.Any()
            ? ClassMapper.ScheduleToScheduleResponse(schedule)
            : throw new BadRequestException($"ClassId: {classId} doesn't exist");
    }

    public async Task UpdateScheduleAsync(ScheduleUpdateRequest dto)
    {
        await CheckPermission();

        var schedules = await _unitOfWork.ScheduleReposisoty.GetScheduleByClassIdAsync(dto.ClassId,ScheduleStatus.AllStatus);

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
                schedule.RoomUrl = dto.RoomUrl;
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
                    RoomUrl = dto.RoomUrl,
                    Days = remainingStudyDays,
                    SlotTime = dto.SlotTime
                };
                await CreateScheduleAsync(entity);
            }
        }
    }
}