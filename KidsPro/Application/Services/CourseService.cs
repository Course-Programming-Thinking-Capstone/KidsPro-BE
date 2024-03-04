using Application.Dtos.Response.Course;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using CreateCourseDto = Application.Dtos.Request.Course.CreateCourseDto;

namespace Application.Services;

public class CourseService : ICourseService
{
    private IUnitOfWork _unitOfWork;
    private IAuthenticationService _authenticationService;
    private IImageService _imageService;
    private ILogger<AccountService> _logger;

    public CourseService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService,
        IImageService imageService, ILogger<AccountService> logger)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
        _imageService = imageService;
        _logger = logger;
    }

    public async Task<CourseDto> GetByIdAsync(int id)
    {
        var course = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} can not found."));

        return CourseMapper.CourseToCourseDto(course);
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto dto)
    {
        var entity = CourseMapper.CreateCourseDtoToEntity(dto);

        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Account not found."));

        entity.CreatedDate = DateTime.UtcNow;
        entity.ModifiedDate = DateTime.UtcNow;
        entity.CreatedBy = currentAccount;
        entity.ModifiedBy = currentAccount;
        entity.Status = CourseStatus.Draft;

        await _unitOfWork.CourseRepository.AddAsync(entity);
        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.CourseToCourseDto(entity);
    }
}