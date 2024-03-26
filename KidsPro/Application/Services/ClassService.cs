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

public class ClassService:IClassService
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
        var account =await _account.GetCurrentAccountInformationAsync();
        
        if (account.Role != Constant.StaffRole && account.Role != Constant.AdminRole)
            throw new UnauthorizedException("Not the staff role, please login by staff account");
        return account;
    }
    public async Task<ClassCreateResponse> CreateClassAsync(ClassCreateRequest dto)
    {
        var account=await CheckPermission();
    
        if (await _unitOfWork.ClassRepository.ExistByClassCode(dto.ClassCode))
            throw new BadRequestException("Class Code has been existed");

        var course = await _unitOfWork.CourseRepository.GetByIdAsync(dto.CourseId)??
            throw new BadRequestException("Course Id not exist");

        var classEntity = new Class()
        {
            Code = dto.ClassCode,
            OpenDate = dto.OpenDay,
            CloseDate = dto.CloseDay,
            Status = ClassStatus.Active,
            CourseId = dto.CourseId,
            CreatedById = account.Id,
            Duration = dto.CloseDay.Month-dto.OpenDay.Month,
            TotalSlot = course.Syllabus?.TotalSlot??0
        };

        await _unitOfWork.ClassRepository.AddAsync(classEntity);
        await _unitOfWork.SaveChangeAsync();

        return ClassMapper.ClassToClassCreateResponse(classEntity, course.Name,course.Syllabus?.SlotTime??0);
    }

    public async Task<ScheduleCreateResponse> CreateScheduleAsync(ScheduleCreateRequest dto)
    {
        await CheckPermission();

        var time = TimeUtils.GetTimeFromSlot(dto.Slot, dto.SlotTime);
        
        var schedule = dto.Days.Select(day => new ClassSchedule
        {
            RoomUrl = dto.Link,
            ClassId = dto.ClassId,
            Slot = dto.Slot,
            StudyDay = (DayStatus)day,
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
                          ??throw new NotFoundException($"ClassId: {classId} doesn't exist");

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

    // public async Task<ClassResponse> GetClassByIdAsync(int classId)
    // {
    //     await CheckPermission();
    //     var entityClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);
    //     entityClass?return C
    // }
}