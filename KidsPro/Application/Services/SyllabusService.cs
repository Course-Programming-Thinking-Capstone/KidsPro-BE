using System.Linq.Expressions;
using Application.Configurations;
using Application.Dtos.Request.Syllabus;
using Application.Dtos.Response.Paging;
using Application.Dtos.Response.Syllabus;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities;
using Domain.Enums;
using Domain.Enums.Status;
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

        if (dto.CourseGameId.HasValue)
        {
            if (!await _unitOfWork.CourseGameRepository.ExistByIdAsync(dto.CourseGameId.Value))
            {
                throw new NotFoundException($"Course game ${dto.CourseGameId} does not exist.");
            }
            course.CourseGameId = dto.CourseGameId.Value;
        }

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

    public async Task<SyllabusDetailDto> GetByIdAsync(int id)
    {
        return await _unitOfWork.SyllabusRepository.GetByIdAsync(id, disableTracking: true)
            .ContinueWith(t => t.Result == null
                ? throw new NotFoundException($"Syllabus {id} not found.")
                : SyllabusMapper.SyllabusToSyllabusDetailDto(t.Result));
    }

    public async Task<PagingResponse<FilterSyllabusDto>> FilterSyllabusAsync(
        string? name,
        SyllabusStatus? status,
        string? sortName,
        string? sortCreatedDate,
        int? page,
        int? size)
    {
        //need to check role
        var parameter = Expression.Parameter(typeof(Syllabus));
        Expression filter = Expression.Constant(true); // default is "where true"

        //set default page size
        if (!page.HasValue || !size.HasValue)
        {
            page = 1;
            size = 10;
        }

        try
        {
            if (!string.IsNullOrEmpty(name))
            {
                filter = Expression.AndAlso(filter,
                    Expression.Call(
                        Expression.Property(parameter, nameof(Syllabus.Name)),
                        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })
                        ?? throw new NotImplementException(
                            $"{nameof(string.Contains)} method is deprecated or not supported."),
                        Expression.Constant(name)));
            }

            if (status.HasValue)
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Syllabus.Status)),
                        Expression.Constant(status.Value)));
            }

            //Default sort by modified date desc
            Func<IQueryable<Syllabus>, IOrderedQueryable<Syllabus>> orderBy = q =>
                q.OrderByDescending(c => c.Course.CreatedDate);

            if (sortName != null && sortName.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.Name);
            }
            else if (sortName != null && sortName.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.Name);
            }
            else if (sortCreatedDate != null && sortCreatedDate.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.Course.CreatedDate);
            }
            else if (sortCreatedDate != null && sortCreatedDate.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.Course.CreatedDate);
            }

            var entities = await _unitOfWork.SyllabusRepository.GetPaginateAsync(
                filter: Expression.Lambda<Func<Syllabus, bool>>(filter, parameter),
                orderBy: orderBy,
                includeProperties: $"{nameof(Syllabus.Course)}",
                page: page,
                size: size
            );
            var result = SyllabusMapper.SyllabusToFilterSyllabusDto(entities);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when execute {methodName} method: \nDetail: {errorDetail}.",
                nameof(this.FilterSyllabusAsync), e.Message);
            throw new Exception($"Error when execute {nameof(this.FilterSyllabusAsync)} method");
        }
    }

    public async Task<PagingResponse<FilterSyllabusDto>> FilterTeacherSyllabusAsync(string? name, string? sortName,
        string? sortCreatedDate, int? page, int? size)
    {
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var parameter = Expression.Parameter(typeof(Syllabus));
        Expression filter = Expression.Constant(true);

        //set default page size
        if (!page.HasValue || !size.HasValue)
        {
            page = 1;
            size = 10;
        }

        try
        {
            filter = Expression.AndAlso(filter,
                Expression.Equal(
                    Expression.Property(
                        Expression.Property(parameter, nameof(Syllabus.Course)),
                        nameof(Course.ModifiedById)),
                    Expression.Constant(accountId, typeof(int?))));
            
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(Syllabus.Status)),
                    Expression.Constant(SyllabusStatus.Open)));

            if (!string.IsNullOrEmpty(name))
            {
                filter = Expression.AndAlso(filter,
                    Expression.Call(
                        Expression.Property(parameter, nameof(Syllabus.Name)),
                        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })
                        ?? throw new NotImplementException(
                            $"{nameof(string.Contains)} method is deprecated or not supported."),
                        Expression.Constant(name)));
            }

            //Default sort by modified date desc
            Func<IQueryable<Syllabus>, IOrderedQueryable<Syllabus>> orderBy = q =>
                q.OrderByDescending(c => c.Course.CreatedDate);

            if (sortName != null && sortName.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.Name);
            }
            else if (sortName != null && sortName.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.Name);
            }
            else if (sortCreatedDate != null && sortCreatedDate.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.Course.CreatedDate);
            }
            else if (sortCreatedDate != null && sortCreatedDate.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.Course.CreatedDate);
            }

            var entities = await _unitOfWork.SyllabusRepository.GetPaginateAsync(
                filter: Expression.Lambda<Func<Syllabus, bool>>(filter, parameter),
                orderBy: orderBy,
                includeProperties: $"{nameof(Syllabus.Course)}",
                page: page,
                size: size
            );
            var result = SyllabusMapper.SyllabusToFilterSyllabusDto(entities);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when execute {methodName} method: \nDetail: {errorDetail}.",
                nameof(this.FilterTeacherSyllabusAsync), e.Message);
            throw new Exception($"Error when execute {nameof(this.FilterTeacherSyllabusAsync)} method");
        }
    }

    public async Task<int> GetNumberOfTeacherDraftSyllabusAsync()
    {
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        if (role.Equals(Constant.TeacherRole))
        {
            var result = await _unitOfWork.SyllabusRepository.GetNumberOfDraftSyllabusAsync(accountId);
            return result;
        }

        return 0;
    }
}