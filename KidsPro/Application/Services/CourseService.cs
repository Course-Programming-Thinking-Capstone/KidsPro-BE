using Application.Configurations;
using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Lesson;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.Lesson;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
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

    public async Task<ICollection<SectionComponentNumberDto>> GetSectionComponentNumberAsync()
    {
        var entities = await _unitOfWork.SectionComponentNumberRepository.GetAsync(
            filter: null,
            orderBy: s => s.OrderBy(s => s.Id),
            disableTracking: true
        );

        return CourseMapper.EntityToSectionComponentNumberDto(entities);
    }

    public async Task<ICollection<SectionComponentNumberDto>> UpdateSectionComponentNumberAsync(
        List<UpdateSectionComponentNumberDto> dtos)
    {
        var entities = new List<SectionComponentNumber>();

        foreach (var type in dtos.Select(dto => EnumUtils.ConvertToSectionComponentType(dto.Name)))
        {
            var entity = await _unitOfWork.SectionComponentNumberRepository.GetByTypeAsync(type)
                .ContinueWith(t => t.Result ?? throw new NotFoundException($"Type {type.ToString()} does not exist"));
            entities.Add(entity);
        }

        _unitOfWork.SectionComponentNumberRepository.UpdateRange(entities);
        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.EntityToSectionComponentNumberDto(entities);
    }

    public async Task RemoveSectionAsync(int id)
    {
        var entity = await _unitOfWork.SectionRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Section {id} not found."));

        _unitOfWork.SectionRepository.Delete(entity);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<LessonDto> AddVideoAsync(int sectionId, CreateVideoDto dto)
    {
        var section = await _unitOfWork.SectionRepository.GetByIdAsync(sectionId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Section {sectionId} does not exist"));

        var sectionVideoNumber =
            await _unitOfWork.SectionComponentNumberRepository.GetByTypeAsync(SectionComponentType.Video);
        if (sectionVideoNumber == null)
        {
            _logger.LogError("Section component type {} can not found.", SectionComponentType.Video);
            throw new Exception($"Section component type {SectionComponentType.Video} can not found.");
        }

        var lessonEntity = CourseMapper.CreateLessonDtoToLesson(dto);
        lessonEntity.SectionId = sectionId;

        var videoNumber = 0;

        foreach (var lesson in section.Lessons)
        {
            if (lesson.Order == dto.Order)
            {
                throw new ConflictException($"Lesson order {dto.Order} has been existed.");
            }

            if (lesson.Type == LessonType.Video)
                videoNumber++;
        }

        if (videoNumber > sectionVideoNumber.MaxNumber)
        {
            throw new BadRequestException(
                $"Can not add more than {sectionVideoNumber.MaxNumber} video in this section.");
        }

        await _unitOfWork.LessonRepository.AddAsync(lessonEntity);
        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.LessonToLessonDto(lessonEntity);
    }

    public async Task<LessonDto> AddDocumentAsync(int sectionId, CreateDocumentDto dto)
    {
        var section = await _unitOfWork.SectionRepository.GetByIdAsync(sectionId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Section {sectionId} does not exist"));

        var sectionDocumentNumber =
            await _unitOfWork.SectionComponentNumberRepository.GetByTypeAsync(SectionComponentType.Document);
        if (sectionDocumentNumber == null)
        {
            _logger.LogError("Section component type {} can not found.", SectionComponentType.Document);
            throw new Exception($"Section component type {SectionComponentType.Document} can not found.");
        }

        var lessonEntity = CourseMapper.CreateLessonDtoToLesson(dto);
        lessonEntity.SectionId = sectionId;

        var documentNumber = 0;

        foreach (var lesson in section.Lessons)
        {
            if (lesson.Order == dto.Order)
            {
                throw new ConflictException($"Lesson order {dto.Order} has been existed.");
            }

            if (lesson.Type == LessonType.Document)
                documentNumber++;
        }

        if (documentNumber > sectionDocumentNumber.MaxNumber)
        {
            throw new BadRequestException(
                $"Can not add more than {sectionDocumentNumber.MaxNumber} document in this section.");
        }

        await _unitOfWork.LessonRepository.AddAsync(lessonEntity);
        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.LessonToLessonDto(lessonEntity);
    }

    public async Task<LessonDto> UpdateVideoAsync(int videoId, UpdateVideoDto dto)
    {
        var video = await _unitOfWork.LessonRepository.GetByIdAsync(videoId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Lesson {videoId} not found."));
        if (video.Type != LessonType.Video)
            throw new BadRequestException("Lesson is not video.");

        CourseMapper.UpdateLessonDtoToLesson(dto, ref video);
        _unitOfWork.LessonRepository.Update(video);
        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.LessonToLessonDto(video);
    }

    public async Task<LessonDto> UpdateDocumentAsync(int documentId, UpdateDocumentDto dto)
    {
        var document = await _unitOfWork.LessonRepository.GetByIdAsync(documentId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Lesson {documentId} not found."));
        if (document.Type != LessonType.Document)
            throw new BadRequestException("Lesson is not document.");

        CourseMapper.UpdateLessonDtoToLesson(dto, ref document);
        _unitOfWork.LessonRepository.Update(document);
        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.LessonToLessonDto(document);
    }

    public async Task<ICollection<LessonDto>> UpdateLessonOrderAsync(List<UpdateLessonOrderDto> dtos)
    {
        var entities = new List<Lesson>();

        foreach (var dto in dtos)
        {
            var entity = await _unitOfWork.LessonRepository.GetByIdAsync(dto.LessonId)
                .ContinueWith(t => t.Result ?? throw new NotFoundException($"Lesson {dto.LessonId} not found."));
            entities.Add(entity);
        }

        _unitOfWork.LessonRepository.UpdateRange(entities);
        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.LessonToLessonDto(entities);
    }
}