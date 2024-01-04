using Application.Configurations;
using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IAuthenticationService _authenticationService;

    private readonly IImageService _imageService;

    private readonly ILogger<CourseService> _logger;

    private readonly string COURSE_PICTURE_FILE_NAME = "Picture";

    private readonly string COURSE_PICTURE_FOLDER = "Image/Course";

    public CourseService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService,
        IImageService imageService,
        ILogger<CourseService> logger)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
        _imageService = imageService;
        _logger = logger;
    }

    public async Task<CourseDto> CreateAsync(CreateCourseDto request)
    {
        var currentUser = await GetCurrentUser();

        //check role 
        if (currentUser.Role.Name != Constant.ADMIN_ROLE && currentUser.Role.Name != Constant.TEACHER_ROLE)
        {
            throw new ForbiddenException("Only teacher or admin can create course.");
        }

        //set data
        var entity = CourseMapper.CreateDtoToEntity(request);
        entity.TotalLesson = 0;
        entity.Status = CourseStatus.Draft;
        entity.IsDelete = false;
        entity.ModifiedDate = DateTime.UtcNow;
        entity.CreatedById = currentUser.Id;
        entity.ModifiedById = currentUser.Id;
        entity.CreatedBy = currentUser;
        entity.ModifiedBy = currentUser;

        await _unitOfWork.CourseRepository.AddAsync(entity);
        try
        {
            var updateResult = await _unitOfWork.SaveChangeAsync();
            return CourseMapper.EntityToDto(entity);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when create course. Detail: {}", e.Message);
            throw new Exception($"Error when create course.");
        }
    }

    public async Task<CourseDto> UpdateAsync(int id, UpdateCourseDto request)
    {
        var currentUser = await GetCurrentUser();

        var entity = await _unitOfWork.CourseRepository.GetAsync(
                filter: c => c.Id == id,
                orderBy: null,
                includeProperties:
                $"{nameof(Course.CreatedBy)},{nameof(Course.ModifiedBy)},{nameof(Course.CourseResources)}",
                disableTracking: false)
            .ContinueWith(t => t.Result.Any()
                ? t.Result.FirstOrDefault() ?? throw new NotFoundException($"Course {id} does not exist.")
                : throw new NotFoundException($"Course {id} does not exist."));

        //check role 
        if (currentUser.Role.Name != Constant.ADMIN_ROLE && currentUser.Id != entity.CreatedById)
        {
            throw new ForbiddenException("Only admin or owner can update this course.");
        }

        if (entity.Status != CourseStatus.Draft)
        {
            throw new BadRequestException("Can just update course with status draft.");
        }

        CourseMapper.UpdateDtoToEntity(request, ref entity);
        entity.ModifiedById = currentUser.Id;
        entity.ModifiedBy = currentUser;
        entity.ModifiedDate = DateTime.UtcNow;

        //Update resource
        if (request.Resources != null)
        {
            //remove old resource
            if (entity.CourseResources != null)
            {
                _unitOfWork.CourseResourceRepository.DeleteRange(entity.CourseResources);
            }
            //Update new resource
            entity.CourseResources = request.Resources.Select(CourseResourceMapper.AddDtoToEntity).ToList();
        }

        _unitOfWork.CourseRepository.Update(entity);

        try
        {
            var updateResult = await _unitOfWork.SaveChangeAsync();
            return CourseMapper.EntityToDto(entity);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when update course {}. Detail: {}", id, e.Message);
            throw new Exception($"Error when update course {id}");
        }
    }

    public async Task<CourseDto> UpdatePictureAsync(int courseId, IFormFile file)
    {
        var currentUser = await GetCurrentUser();

        var course = await _unitOfWork.CourseRepository.GetAsync(
                filter: c => c.Id == courseId,
                orderBy: null,
                includeProperties: $"{nameof(Course.CreatedBy)},{nameof(Course.ModifiedBy)}",
                disableTracking: false)
            .ContinueWith(t => t.Result.Any()
                ? t.Result.FirstOrDefault() ?? throw new NotFoundException($"Course {courseId} does not exist.")
                : throw new NotFoundException($"Course {courseId} does not exist."));

        //check role
        if (currentUser.Role.Name != Constant.ADMIN_ROLE && currentUser.Role.Name != Constant.STAFF_ROLE &&
            currentUser.Role.Name != Constant.TEACHER_ROLE)
        {
            throw new ForbiddenException("Action forbidden.");
        }

        //remove before upload

        if (!string.IsNullOrEmpty(course.PictureUrl))
        {
            await _imageService.RemoveFile(course.PictureUrl);
        }

        var uploadedUrl =
            await _imageService.UploadImage(file, COURSE_PICTURE_FOLDER, COURSE_PICTURE_FILE_NAME);
        course.PictureUrl = uploadedUrl;
        course.ModifiedById = currentUser.Id;
        course.ModifiedBy = currentUser;
        course.ModifiedDate = DateTime.UtcNow;
        _unitOfWork.CourseRepository.Update(course);
        try
        {
            var updateResult = await _unitOfWork.SaveChangeAsync();
            return CourseMapper.EntityToDto(course);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when update picture for course {}. Detail: {}", course, e.Message);
            throw new Exception($"Error when update picture for course {course}");
        }
    }

    private async Task<User> GetCurrentUser()
    {
        var currentUserId = _authenticationService.GetCurrentUserId();

        return await _unitOfWork.UserRepository
            .GetAsync(
                filter: u => u.Id == currentUserId && u.Status == UserStatus.Active,
                orderBy: null,
                includeProperties: $"{nameof(User.Role)}",
                disableTracking: true
            )
            .ContinueWith(t =>
                t.Result.Any()
                    ? t.Result.FirstOrDefault() ?? throw new NotFoundException("User does not exist or being block.")
                    : throw new NotFoundException("User does not exist or being block."));
    }
}