using System.Linq.Expressions;
using Application.Configurations;
using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Request.Progress;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.CourseModeration;
using Application.Dtos.Response.Course.FilterCourse;
using Application.Dtos.Response.Course.Study;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using Domain.Enums.Status;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class CourseService : ICourseService
{
    private IUnitOfWork _unitOfWork;
    private IAuthenticationService _authenticationService;
    private IImageService _imageService;
    private ILogger<AccountService> _logger;
    private IAccountService _accountService;

    public CourseService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService,
        IImageService imageService, ILogger<AccountService> logger, IAccountService accountService)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
        _imageService = imageService;
        _logger = logger;
        _accountService = accountService;
    }

    public async Task<CourseDto> GetByIdAsync(int id, string? action)
    {
        var course = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} can not found."));

        if (action == "manage")
            return CourseMapper.CourseToManageCourseDto(course);
        return CourseMapper.CourseToCommonCourseDto(course);
    }

    public async Task<StudyCourseDto?> GetActiveStudyCourseByIdAsync(int id)
    {
        var statuses = new List<CourseStatus>()
        {
            CourseStatus.Active
        };
        var course = await _unitOfWork.CourseRepository.GetCourseDetailByIdAndStatusAsync(id, statuses);

        return course == null ? null : CourseMapper.CourseToStudyCourseDto(course);
    }

    public async Task<StudyCourseDto?> GetStudyCourseByIdAsync(int id)
    {
        // check authorize
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        Course? entity = null;
        switch (role)
        {
            case Constant.AdminRole:
            case Constant.StaffRole:
            {
                var statuses = new List<CourseStatus>()
                {
                    CourseStatus.Active,
                    CourseStatus.Denied,
                    CourseStatus.Draft,
                    CourseStatus.Inactive,
                    CourseStatus.Pending,
                    CourseStatus.Waiting
                };
                entity = await _unitOfWork.CourseRepository.GetCourseDetailByIdAndStatusAsync(id, statuses);
                break;
            }
            case Constant.TeacherRole:
                entity = await _unitOfWork.CourseRepository.GetTeacherCourseDetailByIdAsync(id, accountId);
                break;
            case Constant.StudentRole:
            {
                entity = await _unitOfWork.CourseRepository.GetStudentCourseDetailByIdAsync(id, accountId);
                if (entity == null)
                    return null;

                var studentProgress =
                    await _unitOfWork.StudentProgressRepository.GetStudentProgressAsync(accountId, id);
                var currentSectionOrder = studentProgress == null ? 1 : studentProgress.Section.Order;
                var result = CourseMapper.CourseToStudyCourseDto(entity, currentSectionOrder);
                return result;
            }
            default:
            {
                var statuses = new List<CourseStatus>()
                {
                    CourseStatus.Active
                };
                entity = await _unitOfWork.CourseRepository.GetCourseDetailByIdAndStatusAsync(id, statuses);
                break;
            }
        }

        return entity != null ? CourseMapper.CourseToStudyCourseDto(entity) : null;
    }

    public async Task<CommonStudySectionDto?> GetActiveCourseStudySectionByIdAsync(int id)
    {
        var courseStatuses = new List<CourseStatus>()
        {
            CourseStatus.Active
        };
        var entity = await _unitOfWork.SectionRepository.GetStudySectionByIdAsync(id, courseStatuses);
        return entity != null ? CourseMapper.SectionToCommonStudySectionDto(entity) : null;
    }

    public async Task<CommonStudySectionDto?> GetSectionDetailByIdAsync(int sectionId)
    {
        // check authorize
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        Section? section = null;

        switch (role)
        {
            case Constant.AdminRole:
            case Constant.StaffRole:
            {
                var statuses = new List<CourseStatus>()
                {
                    CourseStatus.Active,
                    CourseStatus.Inactive,
                    CourseStatus.Denied,
                    CourseStatus.Draft,
                    CourseStatus.Pending,
                    CourseStatus.Waiting
                };
                section = await _unitOfWork.SectionRepository.GetStudySectionByIdAsync(sectionId, statuses);
                break;
            }
            case Constant.TeacherRole:
                section = await _unitOfWork.SectionRepository.GetTeacherSectionDetailByIdAsync(sectionId, accountId);
                break;
            case Constant.StudentRole:
                section = await _unitOfWork.SectionRepository.GetStudentSectionDetailByIdAsync(sectionId, accountId);
                break;
            default:
            {
                var courseStatuses = new List<CourseStatus>()
                {
                    CourseStatus.Active
                };
                section = await _unitOfWork.SectionRepository.GetStudySectionByIdAsync(sectionId, courseStatuses);
                break;
            }
        }

        return section != null ? CourseMapper.SectionToCommonStudySectionDto(section) : null;
    }

    public async Task<StudyLessonDto?> GetFreeStudyLessonByIdAsync(int id)
    {
        var result = await _unitOfWork.LessonRepository.GetCommonLessonDetailByIdAsync(id);
        return result != null ? CourseMapper.LessonToStudyLessonDto(result) : null;
    }

    public async Task<StudyLessonDto?> GetStudyLessonByIdAsync(int lessonId)
    {
        // check authorize
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

        var lesson = role switch
        {
            Constant.AdminRole or Constant.StaffRole => await _unitOfWork.LessonRepository.GetByIdAsync(lessonId),
            Constant.TeacherRole => await _unitOfWork.LessonRepository.GetTeacherLessonDetailByIdAsync(lessonId,
                accountId),
            Constant.StudentRole => await _unitOfWork.LessonRepository.GetStudentLessonDetailByIdAsync(lessonId,
                accountId),
            _ => await _unitOfWork.LessonRepository.GetCommonLessonDetailByIdAsync(lessonId)
        };

        return lesson != null ? CourseMapper.LessonToStudyLessonDto(lesson) : null;
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
        entity.CreatedDate = DateTime.UtcNow;
        entity.ModifiedDate = DateTime.UtcNow;
        entity.CreatedBy = currentAccount;
        entity.ModifiedBy = teacherAccount;
        entity.Status = CourseStatus.Draft;
        entity.Price = 1000;

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
            Title = "Create course success",
            Content = "You have successfully created a new course",
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
        await _unitOfWork.NotificationRepository.AddRangeAsync(new[] { adminNotification, teacherNotification });

        await _unitOfWork.SaveChangeAsync();
        return CourseMapper.CourseToManageCourseDto(entity);
    }

    public async Task<CourseDto> UpdateCourseAsync(int courseId, Dtos.Request.Course.Update.Course.UpdateCourseDto dto,
        string? action)
    {
        // check course
        var courseEntity = await _unitOfWork.CourseRepository.GetByIdAsync(courseId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {courseId} does not exist."));

        if (courseEntity.Status != CourseStatus.Draft && courseEntity.Status != CourseStatus.Denied)
            throw new BadRequestException("Can only update course wih status draft or denied.");

        // check authorize
        _authenticationService.GetCurrentUserInformation(out var accountId, out _);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId, disableTracking: true)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("UserName not found."));

        if (currentAccount.Role.Name != Constant.AdminRole && currentAccount.Id != courseEntity.ModifiedById)
            throw new ForbiddenException("Access denied.");

        //Get Section component number of each type in section

        var sectionVideoNumber = await GetSectionComponentNumberAsync(SectionComponentType.Video);

        var sectionDocumentNumber = await GetSectionComponentNumberAsync(SectionComponentType.Document);

        var sectionQuizNumber = await GetSectionComponentNumberAsync(SectionComponentType.Quiz);

        // update course 
        if (!string.IsNullOrEmpty(dto.Description))
            courseEntity.Description = dto.Description;

        if (dto.Sections != null)
        {
            var totalLesson = courseEntity.TotalLesson;

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

                        //Count total lesson
                        var originNumberSectionLesson = section.Lessons.Count;
                        var updateNumberSectionLesson = sectionDto.Lessons.Count;
                        totalLesson += updateNumberSectionLesson - originNumberSectionLesson;

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
                                QuizMapper.UpdateQuizDtoToQuiz(sectionDtoQuiz, ref quiz);
                            }
                            else
                            {
                                // Create new quiz
                                quiz = QuizMapper.UpdateQuizDtoToQuiz(sectionDtoQuiz);
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
                                        QuizMapper.UpdateQuestionDtoToQuestion(updateQuestionDto, ref question);
                                    }
                                    else
                                    {
                                        question = QuizMapper.UpdateQuestionDtoToQuestion(updateQuestionDto);
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
                                                option = QuizMapper.UpdateOptionDtoToOption(updateOption);
                                            }
                                            else
                                            {
                                                QuizMapper.UpdateOptionDtoToOption(updateOption, ref option);
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

                                //update number os question
                                if (quiz.NumberOfQuestion == 0 || quiz.NumberOfQuestion > quiz.TotalQuestion)
                                {
                                    quiz.NumberOfQuestion = quiz.TotalQuestion;
                                }

                                //Add pass condition to quiz
                                if (courseEntity.Syllabus?.PassConditionId.HasValue ?? false)
                                {
                                    quiz.PassConditionId = courseEntity.Syllabus.PassConditionId;
                                }
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

            //update total lesson
            courseEntity.TotalLesson = totalLesson;
        }

        if (string.IsNullOrEmpty(action) || action.Equals("Save"))
        {
            courseEntity.Status = CourseStatus.Draft;
        }
        else if (action.Equals("Post"))
        {
            courseEntity.Status = CourseStatus.Pending;

            //update syllabus status
            if (courseEntity.Syllabus != null) courseEntity.Syllabus.Status = SyllabusStatus.Pending;

            //Create notification
            var notificationAccounts = await _unitOfWork.AccountRepository.GetAsync(
                filter: a => a.Role.Name == Constant.StaffRole || a.Role.Name == Constant.AdminRole
                    && a.Status == UserStatus.Active && !a.IsDelete,
                orderBy: null,
                includeProperties: $"{nameof(Account.Role)}",
                disableTracking: true
            );

            var adminStaffNotifications = notificationAccounts.Select(t => new UserNotification()
            {
                AccountId = t.Id
            }).ToList();

            var adminStaffNotification = new Notification()
            {
                Title = "Request accept course",
                Content = $"Teacher {currentAccount.FullName} has create a course approval request.",
                Date = DateTime.UtcNow,
                UserNotifications = adminStaffNotifications
            };

            var teacherNotifications = new List<UserNotification>
            {
                new()
                {
                    AccountId = accountId
                }
            };

            var teacherNotification = new Notification()
            {
                Title = "Success create course approval request.",
                Content = "You have create a course approval request. Your course is waiting for processing.",
                Date = DateTime.UtcNow,
                UserNotifications = teacherNotifications
            };

            await _unitOfWork.NotificationRepository.AddRangeAsync(
                new[] { adminStaffNotification, teacherNotification });
        }
        else
        {
            throw new BadRequestException($"Unsupported update course action {action}");
        }

        courseEntity.ModifiedDate = DateTime.UtcNow;

        _unitOfWork.CourseRepository.Update(courseEntity);

        await _unitOfWork.SaveChangeAsync();

        return CourseMapper.CourseToManageCourseDto(courseEntity);
    }

    public async Task ApproveCourseAsync(int id, AcceptCourseDto dto)
    {
        // check authorize
        _authenticationService.GetCurrentUserInformation(out var accountId, out _);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId, disableTracking: true)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("UserName not found."));

        if (currentAccount.Role.Name != Constant.AdminRole && currentAccount.Role.Name != Constant.StaffRole)
        {
            _logger.LogInformation("UserName {} try to access function {}. Date {}", accountId,
                nameof(this.ApproveCourseAsync), DateTime.UtcNow);
            throw new ForbiddenException($"Access denied.");
        }

        var courseEntity = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} not found."));

        switch (currentAccount.Role.Name)
        {
            case Constant.StaffRole when courseEntity.Status != CourseStatus.Pending:
                throw new BadRequestException($"Course {id} is not waiting for approve.");
            case Constant.StaffRole when dto.IsAdminSetup:
                courseEntity.Status = CourseStatus.Waiting;
                break;
            case Constant.StaffRole:
            {
                if (dto.IsFree)
                {
                    courseEntity.IsFree = true;
                }
                else
                {
                    courseEntity.IsFree = false;
                    courseEntity.Price = dto.Price ?? throw new BadRequestException("Course price is missing.");
                }

                courseEntity.ApprovedById = currentAccount.Id;
                courseEntity.ApprovedBy = currentAccount;

                courseEntity.Status = CourseStatus.Active;

                //update syllabus status
                if (courseEntity.Syllabus != null) courseEntity.Syllabus.Status = SyllabusStatus.Active;
                break;
            }
            case Constant.AdminRole:
            {
                if (courseEntity.Status != CourseStatus.Pending && courseEntity.Status != CourseStatus.Waiting)
                    throw new BadRequestException($"Course {id} is not waiting for approve.");
                if (dto.IsFree)
                {
                    courseEntity.IsFree = true;
                }
                else
                {
                    courseEntity.IsFree = false;
                    courseEntity.Price = dto.Price ?? throw new BadRequestException("Course price is missing.");
                }

                courseEntity.ApprovedById = currentAccount.Id;
                courseEntity.ApprovedBy = currentAccount;

                courseEntity.Status = CourseStatus.Active;
                //update syllabus status
                if (courseEntity.Syllabus != null) courseEntity.Syllabus.Status = SyllabusStatus.Active;

                break;
            }
            default:
                throw new ForbiddenException("Access denied.");
        }

        _unitOfWork.CourseRepository.Update(courseEntity);

        //Create notification
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task DenyCourseAsync(int id, string? reason)
    {
        // check authorize
        _authenticationService.GetCurrentUserInformation(out var accountId, out _);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId, disableTracking: true)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("UserName not found."));

        if (currentAccount.Role.Name != Constant.AdminRole && currentAccount.Role.Name != Constant.StaffRole)
        {
            _logger.LogInformation("UserName {} try to access function {}. Date {}", accountId,
                nameof(this.ApproveCourseAsync), DateTime.UtcNow);
            throw new ForbiddenException($"Access denied.");
        }

        var courseEntity = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} not found."));

        switch (currentAccount.Role.Name)
        {
            case Constant.StaffRole:
            {
                if (courseEntity.Status != CourseStatus.Pending)
                    throw new BadRequestException($"Course {id} is not waiting for approve.");
                break;
            }
            case Constant.AdminRole:
            {
                if (courseEntity.Status != CourseStatus.Pending && courseEntity.Status != CourseStatus.Waiting)
                    throw new BadRequestException($"Course {id} is not waiting for approve.");
                break;
            }
            default:
                throw new ForbiddenException("Access denied.");
        }

        courseEntity.Status = CourseStatus.Denied;

        //update syllabus status
        if (courseEntity.Syllabus != null) courseEntity.Syllabus.Status = SyllabusStatus.Open;

        if (courseEntity.ModifiedById == null)
        {
            throw new BadRequestException($"Teacher id in course is null.");
        }

        var teacherNotifications = new List<UserNotification>
        {
            new()
            {
                AccountId = courseEntity.ModifiedById.Value
            }
        };

        var teacherNotification = new Notification()
        {
            Title = "Your course has been denied.",
            Content = $"Reason: {reason}",
            Date = DateTime.UtcNow,
            UserNotifications = teacherNotifications
        };

        await _unitOfWork.NotificationRepository.AddAsync(teacherNotification);

        _unitOfWork.CourseRepository.Update(courseEntity);

        //Create notification
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<CourseDto> CommonUpdateCourseAsync(int id, UpdateCourseDto dto)
    {
        var entity = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} not found."));

        _authenticationService.GetCurrentUserInformation(out var accountId, out _);

        var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(accountId)
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("UserName not found."));

        CourseMapper.UpdateCourseDtoToEntity(dto, ref entity);
        entity.ModifiedDate = DateTime.UtcNow;
        entity.CreatedBy = currentAccount;

        _unitOfWork.CourseRepository.Update(entity);
        await _unitOfWork.SaveChangeAsync();

        return CourseMapper.CourseToManageCourseDto(entity);
    }

    public async Task<string> UpdateCoursePictureAsync(int id, IFormFile file)
    {
        var entity = await _unitOfWork.CourseRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Course {id} not found."));

        var avatarFileName = $"picture_{entity.Id}";

        var uploadedFile = await _imageService.UploadImage(file, Constant.FirebaseCoursePictureFolder, avatarFileName);

        entity.PictureUrl = uploadedFile;

        _unitOfWork.CourseRepository.Update(entity);
        await _unitOfWork.SaveChangeAsync();

        return entity.PictureUrl;
    }

    public async Task<IPagingResponse<FilterCourseDto>> FilterCourseAsync(string? name, CourseStatus? status,
        string? sortName, string? sortCreatedDate,
        string? sortModifiedDate, string? action, int? page, int? size)
    {
        if (action == "manage")
        {
            return await FilterManageCourseAsync(name, status, sortName, sortCreatedDate, sortModifiedDate, page, size);
        }
        else
        {
            return await FilterCommonCourseAsync(name, sortName, page, size);
        }
    }

    public async Task<IPagingResponse<ManageFilterCourseDto>> FilterTeacherCourseAsync(string? name,
        CourseStatus? status, int? page, int? size)
    {
        _authenticationService.GetCurrentUserInformation(out var accountId, out _);
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

            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(Course.ModifiedById)),
                    Expression.Constant(accountId, typeof(int?))));

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
                        Expression.Constant(status.Value)));
            }

            //Default sort by modified date desc
            Func<IQueryable<Course>, IOrderedQueryable<Course>> orderBy = q => q.OrderByDescending(c => c.CreatedDate);

            var entities = await _unitOfWork.CourseRepository.GetPaginateAsync(
                filter: Expression.Lambda<Func<Course, bool>>(filter, parameter),
                orderBy: orderBy,
                page: page,
                size: size
            );
            var result = CourseMapper.CourseToManageFilterCourseDto(entities);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when execute {methodName} method: \nDetail: {errorDetail}.",
                nameof(this.FilterTeacherCourseAsync), e.Message);
            throw new Exception($"Error when execute {nameof(this.FilterTeacherCourseAsync)} method");
        }
    }

    private async Task<PagingResponse<CommonFilterCourseDto>> FilterCommonCourseAsync(
        string? name,
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

            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(Course.Status)),
                    Expression.Constant(CourseStatus.Active)));

            //Default sort by modified date desc
            Func<IQueryable<Course>, IOrderedQueryable<Course>> orderBy = q => q.OrderByDescending(c => c.CreatedDate);

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
            var result = CourseMapper.CourseToCommonFilterCourseDto(entities);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when execute {methodName} method: \nDetail: {errorDetail}.",
                nameof(this.FilterCommonCourseAsync), e.Message);
            throw new Exception($"Error when execute {nameof(this.FilterCommonCourseAsync)} method");
        }
    }

    private async Task<PagingResponse<ManageFilterCourseDto>> FilterManageCourseAsync(
        string? name, CourseStatus? status,
        string? sortName, string? sortCreatedDate,
        string? sortModifiedDate, int? page, int? size)
    {
        _authenticationService.GetCurrentUserInformation(out var accountId, out var role);

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
                if (status.Equals(CourseStatus.Draft))
                {
                    //Check role
                    if (role != Constant.TeacherRole && role != Constant.AdminRole)
                    {
                        _logger.LogWarning("UserName {} is trying to filter pending course.\nDate: {}", accountId,
                            DateTime.UtcNow);
                        throw new ForbiddenException("Access denied.");
                    }

                    if (role == Constant.TeacherRole)
                    {
                        filter = Expression.AndAlso(filter,
                            Expression.Equal(Expression.Property(parameter, nameof(Course.ModifiedById)),
                                Expression.Constant(accountId, typeof(int?))));
                    }
                }

                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Course.Status)),
                        Expression.Constant(status)));
            }

            Func<IQueryable<Course>, IOrderedQueryable<Course>> orderBy = q => q.OrderByDescending(c => c.CreatedDate);

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

            var entities = await _unitOfWork.CourseRepository.GetPaginateAsync(
                filter: Expression.Lambda<Func<Course, bool>>(filter, parameter),
                orderBy: orderBy,
                page: page,
                size: size
            );
            var result = CourseMapper.CourseToManageFilterCourseDto(entities);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when execute {methodName} method: \nDetail: {errorDetail}.",
                nameof(this.FilterManageCourseAsync), e.Message);
            throw new Exception($"Error when execute {nameof(this.FilterManageCourseAsync)} method");
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

    public async Task<CourseOrderDto> GetCoursePaymentAsync(int courseId, int classId)
    {
        var result = await _unitOfWork.CourseRepository.GetCoursePayment(courseId, classId);
        if (result != null)
            return CourseMapper.ShowCoursePayment(result);
        throw new NotFoundException("courseId or classId doesn't exist");
    }

    private async Task<SectionComponentNumber> GetSectionComponentNumberAsync(SectionComponentType type)
    {
        var sectionComponentNumber =
            await _unitOfWork.SectionComponentNumberRepository.GetByTypeAsync(type);
        if (sectionComponentNumber != null) return sectionComponentNumber;
        _logger.LogError("Section component type {} can not found.", type.ToString());
        throw new Exception($"Section component type {type.ToString()} can not found.");
    }

    public async Task StartStudySectionAsync(StudentProgressRequest dto)
    {
        var account = await _accountService.GetCurrentAccountInformationAsync();

        if (await _unitOfWork.StudentProgressRepository
                .CheckStudentSectionExistAsync(account.IdSubRole, dto.SectionId))
        {
            var progress = new StudentProgress()
            {
                SectionId = dto.SectionId,
                StudentId = account.IdSubRole,
                CourseId = dto.CourseId,
                Status = StudentProgressStatus.OnGoing,
                EnrolledDate = DateTime.UtcNow,
            };
            await _unitOfWork.StudentProgressRepository.AddAsync(progress);
            await _unitOfWork.SaveChangeAsync();
            return;
        }

        throw new BaseException($"StudentId {account.IdSubRole} && SectionId {dto.SectionId} are exist");
    }

    public async Task MarkLessonCompletedAsync(int lessonId)
    {
        var account = await _accountService.GetCurrentAccountInformationAsync();

        var student = await _unitOfWork.StudentRepository.GetByIdAsync(account.IdSubRole)
                      ?? throw new BadRequestException($"StudentId {account.IdSubRole} not found");

        var lesson = new StudentLesson()
        {
            LessonId = lessonId,
            StudentId = account.IdSubRole,
            IsCompleted = true
        };

        if (student.StudentLessons.Count == 0)
            student.StudentLessons = new List<StudentLesson>();

        student.StudentLessons.Add(lesson);
        _unitOfWork.StudentRepository.Update(student);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task UpdateToPendingStatus(int courseId, int number)
    {
        var course = await _unitOfWork.CourseRepository.GetByIdAsync(courseId) ??
                     throw new BadRequestException("CourseId not found");
        switch (number)
        {
            case 1:
                course.Status = CourseStatus.Pending;
                break;
            case 2:
                course.Price = 10000;
                break;
        }

        _unitOfWork.CourseRepository.Update(course);
        await _unitOfWork.SaveChangeAsync();
    }

    private async Task<List<Course>> GetCourseByStatusAsync(CourseStatus status)
    {
        return await _unitOfWork.CourseRepository.GetCoursesByStatusAsync(status);
    }

    public async Task<List<CourseModerationResponse>> GetCourseModerationAsync()
    {
        var course = await GetCourseByStatusAsync(CourseStatus.Pending);
        return CourseMapper.CourseToCourseModerationResponse(course);
    }
}