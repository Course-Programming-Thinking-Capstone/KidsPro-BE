using Application.Configurations;
using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
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

    public async Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto dto)
    {
        var entity = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} not found."));

        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Account not found."));

        CourseMapper.UpdateCourseDtoToEntity(dto, ref entity);
        entity.ModifiedDate = DateTime.UtcNow;
        entity.CreatedBy = currentAccount;

        _unitOfWork.CourseRepository.Update(entity);
        await _unitOfWork.SaveChangeAsync();

        return CourseMapper.CourseToCourseDto(entity);
    }

    public async Task<string> UpdateCoursePictureAsync(int id, IFormFile file)
    {
        var entity = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} not found."));

        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException("Can not find account."));

        var avatarFileName = $"picture_{entity.Id}";

        var uploadedFile = await _imageService.UploadImage(file, Constant.FirebaseCoursePictureFolder, avatarFileName);

        entity.PictureUrl = uploadedFile;
        entity.ModifiedBy = account;

        _unitOfWork.CourseRepository.Update(entity);
        await _unitOfWork.SaveChangeAsync();

        return entity.PictureUrl;
    }

    public async Task<SectionDto> CreateSectionAsync(int courseId, CreateSectionDto dto)
    {
        if (await _unitOfWork.SectionRepository.ExistByOrderAsync(courseId, dto.Order))
            throw new ConflictException($"Order {dto.Order} has been existed.");

        var courseEntity = await _unitOfWork.CourseRepository.GetByIdAsync(courseId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {courseId} not found."));

        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException("Invalid token."));

        var entity = new Section()
        {
            Name = dto.Name,
            Order = dto.Order,
            CourseId = courseId
        };

        courseEntity.ModifiedDate = DateTime.UtcNow;
        courseEntity.ModifiedBy = account;

        await _unitOfWork.SectionRepository.AddAsync(entity);
        _unitOfWork.CourseRepository.Update(courseEntity);
        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.SectionToSectionDto(entity);
    }

    public async Task<SectionDto> UpdateSectionAsync(int sectionId, UpdateSectionDto dto)
    {
        var entity = await _unitOfWork.SectionRepository.GetByIdAsync(sectionId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Section {sectionId} can not found."));

        CourseMapper.UpdateSectionDtoToSection(dto, ref entity);
        _unitOfWork.SectionRepository.Update(entity);
        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.SectionToSectionDto(entity);
    }

    public async Task<List<SectionDto>> UpdateSectionOrderAsync(int courseId, List<UpdateSectionOrderDto> dtos)
    {
        if (!await _unitOfWork.CourseRepository.ExistByIdAsync(courseId))
            throw new BadRequestException($"Course {courseId} does not exist.");

        var entities = new List<Section>();

        foreach (var dto in dtos)
        {
            var entity = await _unitOfWork.SectionRepository.GetByIdAsync(dto.Id)
                .ContinueWith(t => t.Result ?? throw new NotFoundException($"Section {dto.Id} not found."));

            if (entity.CourseId != courseId)
                throw new BadRequestException($"Section {dto.Id} do not belong to course {courseId}");

            entity.Order = dto.Order;
            entities.Add(entity);
        }

        _unitOfWork.SectionRepository.UpdateRange(entities);
        await _unitOfWork.SaveChangeAsync();

        return CourseMapper.SectionToSectionDto(entities);
    }
}