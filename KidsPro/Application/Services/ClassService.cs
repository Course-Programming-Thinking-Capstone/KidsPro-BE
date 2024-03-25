using Application.Configurations;
using Application.Dtos.Request.Class;
using Application.Dtos.Response;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
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

    public async Task<ClassCreateResponse> CreateClassAsync(ClassCreateRequest dto)
    {
        var account =await _account.GetCurrentAccountInformationAsync();
    
        if (account.Role != Constant.StaffRole && account.Role != Constant.AdminRole)
            throw new UnauthorizedException("Not the staff role, please login by staff account");
    
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
            Duration = dto.CloseDay.Month-dto.OpenDay.Month
        };

        await _unitOfWork.ClassRepository.AddAsync(classEntity);
        await _unitOfWork.SaveChangeAsync();

        return ClassMapper.ClassToClassCreateResponse(classEntity, course.Name);
    }
    
}