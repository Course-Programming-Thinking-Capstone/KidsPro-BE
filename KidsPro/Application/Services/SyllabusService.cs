using Application.Configurations;
using Application.Dtos.Request.Syllabus;
using Application.Dtos.Response.Syllabus;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class SyllabusService : ISyllabusService
{
    private IUnitOfWork _unitOfWork;
    private IAuthenticationService _authenticationService;
    private IImageService _imageService;
    private ILogger<AccountService> _logger;


    public SyllabusService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService,
        IImageService imageService, ILogger<AccountService> logger)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
        _imageService = imageService;
        _logger = logger;
    }

    public async Task<SyllabusDetailDto> CreateAsync(CreateSyllabusDto dto)
    {
        var entity = new Syllabus();

        _authenticationService.GetCurrentUserInformation(out var accountId, out _);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId, disableTracking: true)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("UserName not found."));

        var teacherAccount = await _unitOfWork.AccountRepository.GetByIdAsync(dto.TeacherId, disableTracking: true)
            .ContinueWith(t =>
                t.Result ?? throw new NotFoundException($"Teacher account {dto.TeacherId} not found."));

        //Check teacher role
        if (teacherAccount.Role.Name != Constant.TeacherRole)
        {
            throw new BadRequestException("Id is not teacher.");
        }

        entity.Name = dto.Name;
        entity.Target = dto.Target;
        entity.TotalSlot = dto.TotalSlot;
        entity.SlotTime = dto.SlotTime;

        //update syllabus status
        entity.Status = SyllabusStatus.Open;

        //add course
        var course = new Course()
        {
            Name = dto.Name,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            CreatedById = accountId,
            ModifiedById = dto.TeacherId,
            Status = CourseStatus.Draft
        };

        //Add section
        if (dto.Sections != null)
        {
            var sections = new List<Section>();

            var sectionIndex = 1;

            foreach (var section in dto.Sections.Select(sectionDto => new Section
                     {
                         Name = sectionDto.Name,
                         Order = sectionIndex
                     }))
            {
                section.Order = sectionIndex;

                sections.Add(section);
                sectionIndex++;
            }

            course.Sections = sections;
        }

        entity.Course = course;

        //Add pass condition
        if (dto.MinQuizScoreRatio.HasValue)
        {
            var passCondition = await _unitOfWork.PassConditionRepository
                .GetByPassRatioAsync(dto.MinQuizScoreRatio.Value)
                .ContinueWith(t => t.Result ?? throw new BadRequestException("Invalid quiz score ratio."));
            entity.PassConditionId = passCondition.Id;
            entity.PassCondition = passCondition;
        }

        //create notification for admin
        var adminUserNotifications = new List<UserNotification>
        {
            new()
            {
                AccountId = accountId
            }
        };

        var adminNotification = new Notification()
        {
            Title = "Create syllabus success",
            Content = "You have successfully created a new syllabus",
            Date = DateTime.UtcNow,
            UserNotifications = adminUserNotifications
        };

        var teacherUserNotifications = new List<UserNotification>
        {
            new()
            {
                AccountId = dto.TeacherId
            }
        };

        var teacherNotification = new Notification()
        {
            Title = "Create new course",
            Content = "You have been designate to create content for new course by admin.",
            Date = DateTime.UtcNow,
            UserNotifications = teacherUserNotifications
        };
        await _unitOfWork.SyllabusRepository.AddAsync(entity);
        await _unitOfWork.NotificationRepository.AddRangeAsync(new[] { adminNotification, teacherNotification });

        await _unitOfWork.SaveChangeAsync();
        return SyllabusMapper.SyllabusToSyllabusDetailDto(entity);
    }
}