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

    private readonly string COURSE_PICTURE_NAME = "Picture";

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
                includeProperties: $"{nameof(Course.CreatedBy)},{nameof(Course.ModifiedBy)}",
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

    public async Task<string> UpdatePicture(IFormFile file)
    {
        //remove before upload

        string imageUrl = "https://firebasestorage.googleapis.com/v0/b/kid-pro.appspot.com/o/Image%2FCourse%2FPicture.jpg?alt=media&token=15c13c53-ed8c-4a62-b8f1-f73b10b27a8b";
        
        if (!string.IsNullOrEmpty(imageUrl))
        {
            await _imageService.RemoveFile(imageUrl);
        }


        return await _imageService.UploadImage(file, Constant.FIREBASE_COURSE_PICTURE_FOLDER, COURSE_PICTURE_NAME);
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