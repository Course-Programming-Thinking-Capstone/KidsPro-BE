using System.Linq.Expressions;
using Application.Configurations;
using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
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

    public async Task<CourseDto> UpdateCourseAsync(int id, Dtos.Request.Course.Update.Course.UpdateCourseDto dto,
        string? action)
    {
        // check course
        var courseEntity = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} does not exist."));

        if (courseEntity.Status != CourseStatus.Draft)
            throw new BadRequestException("Can only update course wih status draft.");

        // check authorize
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId, disableTracking: true)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Account not found."));

        if (currentAccount.Role.Name != Constant.AdminRole && currentAccount.Id != courseEntity.ModifiedById)
        {
            throw new ForbiddenException("Access denied.");
        }

        //Get Section component number of each type in section
        var sectionVideoNumber =
            await _unitOfWork.SectionComponentNumberRepository.GetByTypeAsync(SectionComponentType.Video);
        if (sectionVideoNumber == null)
        {
            _logger.LogError("Section component type {} can not found.", SectionComponentType.Video);
            throw new Exception($"Section component type {SectionComponentType.Video} can not found.");
        }

        var sectionDocumentNumber =
            await _unitOfWork.SectionComponentNumberRepository.GetByTypeAsync(SectionComponentType.Document);
        if (sectionDocumentNumber == null)
        {
            _logger.LogError("Section component type {} can not found.", SectionComponentType.Document);
            throw new Exception($"Section component type {SectionComponentType.Document} can not found.");
        }

        var sectionQuizNumber =
            await _unitOfWork.SectionComponentNumberRepository.GetByTypeAsync(SectionComponentType.Quiz);
        if (sectionQuizNumber == null)
        {
            _logger.LogError("Section component type {} can not found.", SectionComponentType.Quiz);
            throw new Exception($"Section component type {SectionComponentType.Quiz} can not found.");
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
                        var videoNumber = 0;
                        var documentNumber = 0;
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

                            switch (lessonDto.Type)
                            {
                                case LessonType.Video:
                                {
                                    videoNumber++;
                                    break;
                                }
                                case LessonType.Document:
                                {
                                    documentNumber++;
                                    break;
                                }
                                default:
                                {
                                    _logger.LogError("Lesson type {} does not exist.", lessonDto.Type);
                                    throw new UnsupportedException($"Lesson type {lessonDto.Type} does not exist.");
                                }
                            }

                            lessonOrder++;
                        }

                        //Check validation
                        if (videoNumber > sectionVideoNumber.MaxNumber)
                            throw new BadRequestException(
                                $"Number of video type exceed max number {sectionVideoNumber}");

                        if (documentNumber > sectionDocumentNumber.MaxNumber)
                            throw new BadRequestException(
                                $"Number of document type exceed max number {sectionDocumentNumber}");

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

                        //Check validation
                        if (quizOrder - 1 > sectionQuizNumber.MaxNumber)
                            throw new BadRequestException(
                                $"Number of quiz type exceed max number {sectionQuizNumber}");

                        section.Quizzes = quizzes;
                    }
                }
                else
                {
                    throw new BadRequestException($"Section {sectionDto.Id} does not exist.");
                }
            }
        }

        if (string.IsNullOrEmpty(action) || action.Equals("Save"))
        {
            courseEntity.Status = CourseStatus.Draft;
        }
        else if (action.Equals("Post"))
        {
            courseEntity.Status = CourseStatus.Pending;
        }
        else
        {
            throw new BadRequestException($"Unsupported update course action {action}");
        }

        _unitOfWork.CourseRepository.Update(courseEntity);
        //Create notification

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

    public async Task<ICollection<SectionComponentNumberDto>> GetSectionComponentNumberAsync()
    {
        var entities = await _unitOfWork.SectionComponentNumberRepository.GetAsync(
            filter: null,
            orderBy: s => s.OrderBy(sc => sc.Id),
            disableTracking: true
        );

        return CourseMapper.EntityToSectionComponentNumberDto(entities);
    }

    public async Task<ICollection<SectionComponentNumberDto>> UpdateSectionComponentNumberAsync(
        IEnumerable<UpdateSectionComponentNumberDto> dtos)
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
}