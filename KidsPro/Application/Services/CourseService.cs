using System.Linq.Expressions;
using Application.Configurations;
using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.Section;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static System.Enum;

namespace Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IAuthenticationService _authenticationService;

    private readonly IImageService _imageService;

    private readonly ILogger<CourseService> _logger;

    private const string CoursePictureFileName = "Picture";

    private const string CoursePictureFolder = "Image/Course";

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
        var currentUser = await GetCurrentUserAsync();

        //check role 
        if (currentUser.Role.Name != Constant.AdminRole && currentUser.Role.Name != Constant.TeacherRole)
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
            await _unitOfWork.SaveChangeAsync();
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
        var currentUser = await GetCurrentUserAsync();

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
        if (currentUser.Role.Name != Constant.AdminRole && currentUser.Id != entity.CreatedById)
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
            await _unitOfWork.SaveChangeAsync();
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
        var currentUser = await GetCurrentUserAsync();

        var course = await _unitOfWork.CourseRepository.GetAsync(
                filter: c => c.Id == courseId,
                orderBy: null,
                includeProperties: $"{nameof(Course.CreatedBy)},{nameof(Course.ModifiedBy)}",
                disableTracking: false)
            .ContinueWith(t => t.Result.Any()
                ? t.Result.FirstOrDefault() ?? throw new NotFoundException($"Course {courseId} does not exist.")
                : throw new NotFoundException($"Course {courseId} does not exist."));

        //check role
        if (currentUser.Role.Name != Constant.AdminRole && currentUser.Role.Name != Constant.StaffRole &&
            currentUser.Role.Name != Constant.TeacherRole)
        {
            throw new ForbiddenException("Action forbidden.");
        }

        //remove before upload

        if (!string.IsNullOrEmpty(course.PictureUrl))
        {
            await _imageService.RemoveFile(course.PictureUrl);
        }

        var uploadedUrl =
            await _imageService.UploadImage(file, CoursePictureFolder, CoursePictureFileName);
        course.PictureUrl = uploadedUrl;
        course.ModifiedById = currentUser.Id;
        course.ModifiedBy = currentUser;
        course.ModifiedDate = DateTime.UtcNow;
        _unitOfWork.CourseRepository.Update(course);
        try
        {
            await _unitOfWork.SaveChangeAsync();
            return CourseMapper.EntityToDto(course);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when update picture for course {}. Detail: {}", course, e.Message);
            throw new Exception($"Error when update picture for course {course}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var currentUser = await GetCurrentUserAsync();

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
        if (currentUser.Role.Name != Constant.AdminRole && currentUser.Id != entity.CreatedById)
        {
            throw new ForbiddenException("Only admin or owner can delete this course.");
        }

        //check course status 
        if (entity.Status is not CourseStatus.Draft)
        {
            throw new BadRequestException("Can only delete draft course.");
        }

        _unitOfWork.CourseRepository.Delete(entity);
        try
        {
            await _unitOfWork.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error when delete course {}.\n Detail: {}", id, e.Message);
            throw new Exception($"Error when delete course {id}.\n");
        }
    }

    public async Task<PagingResponse<CommonManageCourseDto>> GetManageCourseAsync(
        string? name,
        string? status,
        string? sortName,
        string? sortCreatedDate,
        string? sortModifiedDate,
        int? page,
        int? size,
        bool isOfCurrentUser = false
    )
    {
        //need to check role
        var parameter = Expression.Parameter(typeof(Course));
        Expression filter = Expression.Constant(true); // default is "where true"

        //set default page size
        if (!page.HasValue || !size.HasValue)
        {
            page = 1;
            size = 10;
        }

        try
        {
            //get course that is not deleted
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(Course.IsDelete)),
                    Expression.Constant(false)));

            if (isOfCurrentUser)
            {
                var currentUser = await GetCurrentUserAsync();

                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Course.CreatedById)),
                        Expression.Constant(currentUser.Id)));
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (!TryParse<CourseStatus>(status, out var statusEnum))
                {
                    throw new BadRequestException($"Invalid status value for status {status}");
                }

                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Course.Status)),
                        Expression.Constant(statusEnum)));
            }

            if (!string.IsNullOrEmpty(name))
            {
                filter = Expression.AndAlso(filter,
                    Expression.Call(
                        Expression.Property(parameter, nameof(Course.Name)),
                        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })
                        ?? throw new NotImplementException(
                            $"{nameof(string.Contains)} method is deprecated or not supported."),
                        Expression.Constant(name)));
            }

            Func<IQueryable<Course>, IOrderedQueryable<Course>> orderBy = q => q.OrderBy(s => s.Id);

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
                orderBy = q => q.OrderBy(s => s.CreatedDate);
            }
            else if (sortCreatedDate != null && sortCreatedDate.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.CreatedDate);
            }
            else if (sortModifiedDate != null && sortModifiedDate.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.ModifiedDate);
            }
            else if (sortModifiedDate != null && sortModifiedDate.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.ModifiedDate);
            }

            var courses = await _unitOfWork.CourseRepository.GetPaginateAsync(
                filter: Expression.Lambda<Func<Course, bool>>(filter, parameter),
                orderBy: orderBy,
                page: page,
                size: size,
                includeProperties: $"{nameof(Course.CreatedBy)}"
            );
            var result = CourseMapper.EntityToCommonManageDto(courses);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when execute GetManageCourseAsync method: \nDetail: {}.", e.Message);
            throw new Exception("Error when execute GetManageCourseAsync method");
        }
    }

    public async Task<PagingResponse<CommonCourseDto>> GetCoursesAsync(string? name, string? sortName,
        string? sortPostedDate,
        int? page, int? size)
    {
        //need to check role
        var parameter = Expression.Parameter(typeof(Course));
        Expression filter = Expression.Constant(true); // default is "where true"

        //set default page size
        if (!page.HasValue || !size.HasValue)
        {
            page = 1;
            size = 10;
        }

        try
        {
            //get course that is not deleted
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(Course.IsDelete)),
                    Expression.Constant(false)));

            //Just get active course
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(Course.Status)),
                    Expression.Constant(CourseStatus.Active)));

            if (!string.IsNullOrEmpty(name))
            {
                filter = Expression.AndAlso(filter,
                    Expression.Call(
                        Expression.Property(parameter, nameof(Course.Name)),
                        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })
                        ?? throw new NotImplementException(
                            $"{nameof(string.Contains)} method is deprecated or not supported."),
                        Expression.Constant(name)));
            }

            Func<IQueryable<Course>, IOrderedQueryable<Course>> orderBy = q => q.OrderBy(s => s.Id);

            if (sortName != null && sortName.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.Name);
            }
            else if (sortName != null && sortName.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.Name);
            }
            else if (sortPostedDate != null && sortPostedDate.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.CreatedDate);
            }
            else if (sortPostedDate != null && sortPostedDate.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.CreatedDate);
            }

            var courses = await _unitOfWork.CourseRepository.GetPaginateAsync(
                filter: Expression.Lambda<Func<Course, bool>>(filter, parameter),
                orderBy: orderBy,
                page: page,
                size: size,
                includeProperties: $"{nameof(Course.CreatedBy)}"
            );
            var result = CourseMapper.EntityToCommonCourseDto(courses);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when execute {methodName} method: \nDetail: {errorDetail}.",
                nameof(this.GetCoursesAsync), e.Message);
            throw new Exception($"Error when execute {nameof(this.GetCoursesAsync)} method");
        }
    }

    public async Task<CourseDto> AddSectionAsync(int courseId, string sectionName)
    {
        var currentUser = await GetCurrentUserAsync();

        //need to Check role
        
        //Need to check status
        var course = await _unitOfWork.CourseRepository.GetAsync(
                filter: c => c.IsDelete == false && c.Id == courseId,
                orderBy: null,
                includeProperties: $"{nameof(Course.ModifiedBy)}",
                disableTracking: false)
            .ContinueWith(t => t.Result.Any()
                ? t.Result.FirstOrDefault() ?? throw new NotFoundException($"Course {courseId} does not exist.")
                : throw new NotFoundException($"Course {courseId} does not exist."));

        // check authorize
        if (currentUser.Role.Name == Constant.TeacherRole && course.CreatedById != currentUser.Id)
        {
            throw new ForbiddenException("Only admin, staff or owner can modify this course.");
        }
        
        var largestOrderCourseSection = await _unitOfWork.CourseSectionRepository.GetFirstByFilterAsync(
            filter: c => c.CourseId == courseId,
            orderBy: c => c.OrderByDescending(t => t.Order),
            disableTracking: true
        );

        var order = largestOrderCourseSection?.Order + 1 ?? 1;

        var courseSectionEntity = new CourseSection()
        {
            Name = sectionName,
            Order = order,
            CourseId = courseId
        };

        await _unitOfWork.CourseSectionRepository.AddAsync(courseSectionEntity);

        course.ModifiedById = currentUser.Id;
        course.ModifiedBy = currentUser;
        course.ModifiedDate = DateTime.UtcNow;

        _unitOfWork.CourseRepository.Update(course);

        try
        {
            await _unitOfWork.SaveChangeAsync();
            return CourseMapper.EntityToDto(course);
        }
        catch (Exception e)
        {
            _logger.LogError("Error when add course section. Detail: {}", e.Message);
            throw new Exception("Error when add course section.");
        }
    }

    public Task<CourseDto> RemoveSectionAsync(int courseId, int courseSectionId)
    {
        throw new NotImplementedException();
    }

    public Task<CourseDto> UpdateSectionOrderAsync(int courseId, List<SectionDto> sections)
    {
        throw new NotImplementedException();
    }

    public async Task<CourseDto> AddSectionAsync(int courseId, List<SectionDto> sections)
    {
        // check role
        var currentUser = await GetCurrentUserAsync();

        var course = await _unitOfWork.CourseRepository.GetAsync(
                filter: c => c.IsDelete == false && c.Id == courseId,
                orderBy: null,
                includeProperties: $"{nameof(Course.CourseSections)},{nameof(Course.ModifiedBy)}",
                disableTracking: false)
            .ContinueWith(t => t.Result.Any()
                ? t.Result.FirstOrDefault()
                : throw new NotFoundException($"Course {courseId} does not exist."));

        //Remove course section
        await _unitOfWork.CourseSectionRepository.DeleteByCourseIdAsync(courseId);


        throw new NotImplementedException();
    }

    private async Task<User> GetCurrentUserAsync()
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