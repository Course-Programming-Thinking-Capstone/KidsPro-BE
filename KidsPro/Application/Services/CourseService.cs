using System.Linq.Expressions;
using Application.Configurations;
using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Lesson;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.Lesson;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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

    public async Task<CourseDto> CreateCourseOldAsync(CreateCourseDto dto)
    {
        var entity = new Course();

        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Account not found."));

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.CreatedDate = DateTime.UtcNow;
        entity.ModifiedDate = DateTime.UtcNow;
        entity.CreatedBy = currentAccount;
        entity.ModifiedBy = currentAccount;
        entity.Status = CourseStatus.Draft;

        if (dto.Sections != null)
        {
            var sections = new List<Section>();

            var sectionIndex = 1;

            foreach (var sectionDto in dto.Sections)
            {
                var section = new Section
                {
                    Name = sectionDto.Name,
                    Order = sectionIndex
                };

                if (sectionDto.Lessons != null)
                {
                    var lessons = new List<Lesson>();
                    var seenOrders = new HashSet<int>();
                    foreach (var lessonDto in sectionDto.Lessons)
                    {
                        if (!seenOrders.Add(lessonDto.Order))
                        {
                            // Duplicate order detected
                            _logger.LogError("Error at {}: \nDuplicate order {} found within lessons.",
                                nameof(CreateCourseOldAsync), lessonDto.Order);
                            throw new BadRequestException($"Duplicate order '{lessonDto.Order}' found within lessons.");
                        }

                        var lesson = CourseMapper.CreateLessonDtoToLesson(lessonDto);
                        lessons.Add(lesson);
                    }

                    section.Lessons = lessons;
                }

                if (sectionDto.Quizzes != null)
                {
                    var quizzes = new List<Quiz>();
                    var quizIndex = 1;
                    foreach (var sectionDtoQuiz in sectionDto.Quizzes)
                    {
                        var quiz = CourseMapper.CreateQuizDtoToQuiz(sectionDtoQuiz);
                        var questions = new List<Question>();
                        var questionOrder = 1;
                        decimal totalScore = 0;

                        foreach (var createQuestionDto in sectionDtoQuiz.Questions)
                        {
                            var question = CourseMapper.CreateQuestionDtoToQuestion(createQuestionDto);
                            question.Order = questionOrder;
                            totalScore += question.Score;

                            var options = new List<Option>();
                            var optionOrder = 1;
                            foreach (var createOptionDto in createQuestionDto.Options)
                            {
                                var option = CourseMapper.CreateOptionDtoToOption(createOptionDto);
                                option.Order = optionOrder;
                                options.Add(option);
                                optionOrder++;
                            }

                            question.Options = options;
                            questions.Add(question);
                            questionOrder++;
                        }

                        if (sectionDtoQuiz.NumberOfQuestion.HasValue)
                        {
                            quiz.NumberOfQuestion = sectionDtoQuiz.NumberOfQuestion.Value;
                        }
                        else
                        {
                            quiz.NumberOfQuestion = questionOrder - 1;
                        }

                        quiz.TotalScore = totalScore;
                        quiz.TotalQuestion = questionOrder - 1;
                        quiz.Questions = questions;
                        quiz.CreatedDate = DateTime.UtcNow;
                        quiz.CreatedById = accountId;
                        quiz.CreatedBy = currentAccount;
                        quiz.Order = quizIndex;

                        quizzes.Add(quiz);
                        quizIndex++;
                    }

                    section.Quizzes = quizzes;
                }

                section.Order = sectionIndex;

                sections.Add(section);
                sectionIndex++;
            }

            entity.Sections = sections;
        }

        await _unitOfWork.CourseRepository.AddAsync(entity);
        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.CourseToCourseDto(entity);
    }

    /// <summary>
    /// Admin create course with exist section and assign teacher to edit course
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BadRequestException"></exception>
    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto dto)
    {
        var entity = new Course();

        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId, disableTracking: true)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Account not found."));

        var teacherAccount = await _unitOfWork.AccountRepository.GetByIdAsync(dto.TeacherId, disableTracking: true)
            .ContinueWith(t =>
                t.Result ?? throw new NotFoundException($"Teacher account {dto.TeacherId} not found."));

        //Check teacher role
        if (teacherAccount.Role.Name != Constant.TeacherRole)
        {
            throw new BadRequestException("Id is not teacher.");
        }

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.CreatedDate = DateTime.UtcNow;
        entity.ModifiedDate = DateTime.UtcNow;
        entity.CreatedBy = currentAccount;
        entity.ModifiedBy = teacherAccount;
        entity.Status = CourseStatus.Draft;

        if (dto.Sections != null)
        {
            var sections = new List<Section>();

            var sectionIndex = 1;

            foreach (var sectionDto in dto.Sections)
            {
                var section = new Section
                {
                    Name = sectionDto.Name,
                    Order = sectionIndex
                };

                section.Order = sectionIndex;

                sections.Add(section);
                sectionIndex++;
            }

            entity.Sections = sections;
        }

        await _unitOfWork.CourseRepository.AddAsync(entity);

        //need to create notification for admin and teacher

        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.CourseToCourseDto(entity);
    }

    public async Task<CourseDto> UpdateCourseAsync(int id, Dtos.Request.Course.Update.Course.UpdateCourseDto dto)
    {
        // check course
        var courseEntity = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} does not exist."));
        // check authorize
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId, disableTracking: true)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Account not found."));

        if (currentAccount.Role.Name != Constant.AdminRole && currentAccount.Id != courseEntity.ModifiedById)
        {
            throw new ForbiddenException($"Access denied.");
        }

        // update course 
        if (dto.Sections != null)
        {
            foreach (var sectionDto in dto.Sections)
            {
                var section = courseEntity.Sections.FirstOrDefault(s => s.Id == sectionDto.Id);

                if (section != null)
                {
                    //update section lesson
                    if (sectionDto.Lessons != null)
                    {
                        var lessons = new List<Lesson>();
                        var lessonOrder = 1;
                        foreach (var lessonDto in sectionDto.Lessons)
                        {
                            if (!lessonDto.Id.HasValue)
                            {
                                var lesson = CourseMapper.UpdateLessonDtoToLesson(lessonDto);
                                lesson.Order = lessonOrder;
                                lessons.Add(lesson);
                            }
                            else
                            {
                                var lessonToUpdate = section.Lessons.FirstOrDefault(l => l.Id == lessonDto.Id);
                                if (lessonToUpdate != null)
                                {
                                    CourseMapper.UpdateLessonDtoToLesson(lessonDto, ref lessonToUpdate);
                                    lessons.Add(lessonToUpdate);
                                }
                                else
                                {
                                    throw new BadRequestException($"Lesson {lessonDto.Id} does not exist.");
                                }
                            }

                            lessonOrder++;
                        }

                        //Assign new lesson to course section
                        section.Lessons = lessons;
                    }

                    //update section quiz
                    if (sectionDto.Quizzes != null)
                    {
                        var quizzes = new List<Quiz>();
                        var quizOrder = 1;
                        foreach (var sectionDtoQuiz in sectionDto.Quizzes)
                        {
                            Quiz quiz;
                            decimal totalScore = 0;

                            if (sectionDtoQuiz.Id.HasValue)
                            {
                                quiz = section.Quizzes.FirstOrDefault(q => q.Id == sectionDtoQuiz.Id);
                                if (quiz == null)
                                    throw new NotFoundException($"Quiz {sectionDtoQuiz.Id} does not exist.");

                                //Update quiz information
                                CourseMapper.UpdateQuizDtoToQuiz(sectionDtoQuiz, ref quiz);
                            }
                            else
                            {
                                // Create new quiz
                                quiz = CourseMapper.UpdateQuizDtoToQuiz(sectionDtoQuiz);
                                quiz.CreatedDate = DateTime.UtcNow;
                                quiz.CreatedById = accountId;
                                quiz.CreatedBy = currentAccount;
                            }

                            // Update quiz question
                            if (sectionDtoQuiz.Questions != null)
                            {
                                var questions = new List<Question>();
                                var questionOrder = 1;

                                foreach (var updateQuestionDto in sectionDtoQuiz.Questions)
                                {
                                    var question = quiz.Questions.FirstOrDefault(q => q.Id == updateQuestionDto.Id);
                                    if (question != null)
                                    {
                                        //Update section information
                                        CourseMapper.UpdateQuestionDtoToQuestion(updateQuestionDto, ref question);
                                    }
                                    else
                                    {
                                        question = CourseMapper.UpdateQuestionDtoToQuestion(updateQuestionDto);
                                    }

                                    // update total score
                                    totalScore += question.Score;

                                    if (updateQuestionDto.Options != null)
                                    {
                                        var options = new List<Option>();
                                        var optionOrder = 1;

                                        foreach (var updateOption in updateQuestionDto.Options)
                                        {
                                            var option =
                                                question.Options.FirstOrDefault(o => o.Id == updateOption.Id);
                                            if (option == null)
                                            {
                                                option = CourseMapper.UpdateOptionDtoToOption(updateOption);
                                            }
                                            else
                                            {
                                                CourseMapper.UpdateOptionDtoToOption(updateOption, ref option);
                                            }

                                            option.Order = optionOrder;
                                            options.Add(option);
                                            optionOrder++;
                                        }

                                        question.Options = options;
                                    }

                                    question.Order = questionOrder;
                                    questions.Add(question);
                                    questionOrder++;
                                }

                                // Update quiz
                                quiz.TotalScore = totalScore;
                                quiz.TotalQuestion = questionOrder - 1;
                                quiz.Questions = questions;
                            }

                            quiz.Order = quizOrder;
                            quizzes.Add(quiz);
                            quizOrder++;
                        }

                        section.Quizzes = quizzes;
                    }
                }
                else
                {
                    throw new BadRequestException($"Section {sectionDto.Id} does not exist.");
                }
            }
        }

        courseEntity.Status = CourseStatus.Pending;

        _unitOfWork.CourseRepository.Update(courseEntity);
        await _unitOfWork.SaveChangeAsync();

        return CourseMapper.CourseToCourseDto(courseEntity);
    }

    public async Task ApproveCourseAsync(int id)
    {
        // check authorize
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId, disableTracking: true)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Account not found."));

        if (currentAccount.Role.Name != Constant.AdminRole && currentAccount.Role.Name != Constant.StaffRole)
        {
            _logger.LogInformation("Account {} try to access function {}. Date {}", accountId,
                nameof(this.ApproveCourseAsync), DateTime.UtcNow);
            throw new ForbiddenException($"Access denied.");
        }

        var courseEntity = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} not found."));

        if (courseEntity.Status != CourseStatus.Pending)
            throw new BadRequestException($"Course {id} is not waiting for approve.");
        courseEntity.Status = CourseStatus.Active;

        _unitOfWork.CourseRepository.Update(courseEntity);

        //Create notification
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task DenyCourseAsync(int id, string? reason)
    {
        // check authorize
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId, disableTracking: true)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Account not found."));

        if (currentAccount.Role.Name != Constant.AdminRole && currentAccount.Role.Name != Constant.StaffRole)
        {
            _logger.LogInformation("Account {} try to access function {}. Date {}", accountId,
                nameof(this.ApproveCourseAsync), DateTime.UtcNow);
            throw new ForbiddenException($"Access denied.");
        }

        var courseEntity = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} not found."));

        if (courseEntity.Status != CourseStatus.Pending)
            throw new BadRequestException($"Course {id} is not waiting for approve.");

        courseEntity.Status = CourseStatus.Denied;

        _unitOfWork.CourseRepository.Update(courseEntity);

        //Create notification
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<CourseDto> CommonUpdateCourseAsync(int id, UpdateCourseDto dto)
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

        var avatarFileName = $"picture_{entity.Id}";

        var uploadedFile = await _imageService.UploadImage(file, Constant.FirebaseCoursePictureFolder, avatarFileName);

        entity.PictureUrl = uploadedFile;

        _unitOfWork.CourseRepository.Update(entity);
        await _unitOfWork.SaveChangeAsync();

        return entity.PictureUrl;
    }

    public async Task<PagingResponse<FilterCourseDto>> FilterCourseAsync(string? name, CourseStatus? status,
        string? sortName, int? page, int? size)
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
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(Course.IsDelete)),
                    Expression.Constant(false)));

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

            if (status.HasValue)
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Course.Status)),
                        Expression.Constant(status)));
            }

            Func<IQueryable<Course>, IOrderedQueryable<Course>> orderBy = q => q.OrderBy(c => c.Id);

            if (sortName != null && sortName.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.Name);
            }
            else if (sortName != null && sortName.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.Name);
            }

            var entities = await _unitOfWork.CourseRepository.GetPaginateAsync(
                filter: Expression.Lambda<Func<Course, bool>>(filter, parameter),
                orderBy: orderBy,
                page: page,
                size: size
            );
            var result = CourseMapper.CourseToFilterCourseDto(entities);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when execute {methodName} method: \nDetail: {errorDetail}.",
                nameof(this.FilterCourseAsync), e.Message);
            throw new Exception($"Error when execute {nameof(this.FilterCourseAsync)} method");
        }
    }

    public async Task<SectionDto> CreateSectionAsync(int courseId, CreateSectionDto dto)
    {
        // if (await _unitOfWork.SectionRepository.ExistByOrderAsync(courseId, dto.Order))
        //     throw new ConflictException($"Order {dto.Order} has been existed.");
        //
        // var courseEntity = await _unitOfWork.CourseRepository.GetByIdAsync(courseId)
        //     .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {courseId} not found."));
        //
        // _authenticationService.GetCurrentUserInformation(out var accountId, out var role);
        //
        // var account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId)
        //     .ContinueWith(t => t.Result ?? throw new NotFoundException("Invalid token."));
        //
        // var entity = new Section()
        // {
        //     Name = dto.Name,
        //     Order = dto.Order,
        //     CourseId = courseId
        // };
        //
        // courseEntity.ModifiedDate = DateTime.UtcNow;
        // courseEntity.ModifiedBy = account;
        //
        // await _unitOfWork.SectionRepository.AddAsync(entity);
        // _unitOfWork.CourseRepository.Update(courseEntity);
        // await _unitOfWork.SaveChangeAsync();
        // return CourseMapper.SectionToSectionDto(entity);

        throw new NotImplementedException();
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

    // public async Task<LessonDto> AddVideoAsync(int sectionId, CreateVideoDto dto)
    // {
    //     var section = await _unitOfWork.SectionRepository.GetByIdAsync(sectionId)
    //         .ContinueWith(t => t.Result ?? throw new NotFoundException($"Section {sectionId} does not exist"));
    //
    //     var sectionVideoNumber =
    //         await _unitOfWork.SectionComponentNumberRepository.GetByTypeAsync(SectionComponentType.Video);
    //     if (sectionVideoNumber == null)
    //     {
    //         _logger.LogError("Section component type {} can not found.", SectionComponentType.Video);
    //         throw new Exception($"Section component type {SectionComponentType.Video} can not found.");
    //     }
    //
    //     var lessonEntity = CourseMapper.CreateLessonDtoToLesson(dto);
    //     lessonEntity.SectionId = sectionId;
    //
    //     var videoNumber = 0;
    //
    //     foreach (var lesson in section.Lessons)
    //     {
    //         if (lesson.Order == dto.Order)
    //         {
    //             throw new ConflictException($"Lesson order {dto.Order} has been existed.");
    //         }
    //
    //         if (lesson.Type == LessonType.Video)
    //             videoNumber++;
    //     }
    //
    //     if (videoNumber > sectionVideoNumber.MaxNumber)
    //     {
    //         throw new BadRequestException(
    //             $"Can not add more than {sectionVideoNumber.MaxNumber} video in this section.");
    //     }
    //
    //     await _unitOfWork.LessonRepository.AddAsync(lessonEntity);
    //     await _unitOfWork.SaveChangeAsync();
    //     return CourseMapper.LessonToLessonDto(lessonEntity);
    // }
    //
    // public async Task<LessonDto> AddDocumentAsync(int sectionId, CreateDocumentDto dto)
    // {
    //     var section = await _unitOfWork.SectionRepository.GetByIdAsync(sectionId)
    //         .ContinueWith(t => t.Result ?? throw new NotFoundException($"Section {sectionId} does not exist"));
    //
    //     var sectionDocumentNumber =
    //         await _unitOfWork.SectionComponentNumberRepository.GetByTypeAsync(SectionComponentType.Document);
    //     if (sectionDocumentNumber == null)
    //     {
    //         _logger.LogError("Section component type {} can not found.", SectionComponentType.Document);
    //         throw new Exception($"Section component type {SectionComponentType.Document} can not found.");
    //     }
    //
    //     var lessonEntity = CourseMapper.CreateLessonDtoToLesson(dto);
    //     lessonEntity.SectionId = sectionId;
    //
    //     var documentNumber = 0;
    //
    //     foreach (var lesson in section.Lessons)
    //     {
    //         if (lesson.Order == dto.Order)
    //         {
    //             throw new ConflictException($"Lesson order {dto.Order} has been existed.");
    //         }
    //
    //         if (lesson.Type == LessonType.Document)
    //             documentNumber++;
    //     }
    //
    //     if (documentNumber > sectionDocumentNumber.MaxNumber)
    //     {
    //         throw new BadRequestException(
    //             $"Can not add more than {sectionDocumentNumber.MaxNumber} document in this section.");
    //     }
    //
    //     await _unitOfWork.LessonRepository.AddAsync(lessonEntity);
    //     await _unitOfWork.SaveChangeAsync();
    //     return CourseMapper.LessonToLessonDto(lessonEntity);
    // }

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