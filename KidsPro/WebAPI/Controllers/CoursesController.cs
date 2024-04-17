using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Quiz;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Request.Progress;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.CourseModeration;
using Application.Dtos.Response.Course.FilterCourse;
using Application.Dtos.Response.Course.Quiz;
using Application.Dtos.Response.Course.Study;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Constant = Application.Configurations.Constant;
using CourseDto = Application.Dtos.Response.Course.CourseDto;
using UpdateCourseDto = Application.Dtos.Request.Course.Update.Course.UpdateCourseDto;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/courses")]
public class CoursesController : ControllerBase
{
    private ICourseService _courseService;
    private IQuizService _quizService;

    public CoursesController(ICourseService courseService, IQuizService quizService)
    {
        _courseService = courseService;
        _quizService = quizService;
    }

    /// <summary>
    /// Get by course Id, param action (can be null or "manage") is used to define whether api used for common user (parent, guest) view course detail or
    /// for managing purpose.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> GetByIdAsync([FromRoute] int id, [FromQuery] string? action)
    {
        var result = await _courseService.GetByIdAsync(id, action);
        return Ok(result);
    }

    /// <summary>
    /// Admin, staff, teacher filter course on the system. action can be null or "manage". If action is null, the api is use
    /// for common user/ parent to view course on system (no need to login to use api). If action is "manage", filter is used for
    /// manage course on the system (require login).
    /// </summary>
    /// <param name="name"></param>
    /// <param name="status"></param>
    /// <param name="sortName"></param>
    /// <param name="action"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="sortCreatedDate"></param>
    /// <param name="sortModifiedDate"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagingResponse<FilterCourseDto>))]
    public async Task<ActionResult<IPagingResponse<FilterCourseDto>>> FilterCourseAsync(
        [FromQuery] string? name,
        [FromQuery] CourseStatus? status,
        [FromQuery] string? sortName,
        [FromQuery] string? sortCreatedDate,
        [FromQuery] string? sortModifiedDate,
        [FromQuery] string? action,
        [FromQuery] int? page,
        [FromQuery] int? size)
    {
        var result = await _courseService.FilterCourseAsync(
            name,
            status,
            sortName,
            sortCreatedDate,
            sortModifiedDate,
            action,
            page,
            size);
        return Ok(result);
    }

    /// <summary>
    /// Create course 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = $"{Constant.AdminRole}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> CreateCourseAsync([FromBody] CreateCourseDto dto)
    {
        var result = await _courseService.CreateCourseAsync(dto);
        return Created(nameof(CreateCourseDto), result);
    }

    /// <summary>
    /// Update course. Teacher can save course as draft or post course base on the value of action parameter:
    /// "Save": save as draft; "Post" create post request. Can not update after create post request
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="action"></param>
    /// <param name="videoFiles"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole}")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> UpdateCourseAsync([FromRoute] int id, [FromBody] UpdateCourseDto dto,
        [FromQuery] string? action)

    {
        var result = await _courseService.UpdateCourseAsync(id, dto, action);
        return Ok();
    }

    /// <summary>
    /// Update course picture
    /// </summary>
    /// <param name="id"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole}")]
    [HttpPatch("{id:int}/picture")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    public async Task<ActionResult<CourseDto>> UpdateCoursePictureAsync([FromRoute] int id, [FromForm] IFormFile file)
    {
        var result = await _courseService.UpdateCoursePictureAsync(id, file);
        return Ok(result);
    }

    /// <summary>
    /// Admin or staff approve pending course
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{id:int}/approve")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.StaffRole}")]
    public async Task<ActionResult> ApproveCourseAsync([FromRoute] int id, [FromBody] AcceptCourseDto dto)
    {
        await _courseService.ApproveCourseAsync(id, dto);
        return Ok();
    }

    /// <summary>
    /// Admin or staff approve pending course
    /// </summary>
    /// <param name="id"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    [HttpPatch("{id:int}/deny")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.StaffRole}")]
    public async Task<ActionResult> DenyCourseAsync([FromRoute] int id, [FromQuery] string? reason)
    {
        await _courseService.DenyCourseAsync(id, reason);
        return Ok();
    }

    /// <summary>
    /// Get Section component number of section in course. Ex a section has at most 5 videos, 3 documents,...
    /// </summary>
    /// <returns></returns>
    [HttpGet("sectionComponentNumber")]
    public async Task<ActionResult<List<SectionComponentNumberDto>>> GetSectionComponentNumberAsync()
    {
        return Ok(await _courseService.GetSectionComponentNumberAsync());
    }

    /// <summary>
    ///  Update Section component number of section in course.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole}")]
    [HttpPut("sectionComponentNumber")]
    public async Task<ActionResult<List<SectionComponentNumberDto>>> UpdateSectionComponentNumberAsync(
        [FromBody] List<UpdateSectionComponentNumberDto> dtos)
    {
        var result = await _courseService.UpdateSectionComponentNumberAsync(dtos);
        return Ok(result);
    }

    /// <summary>
    /// Get thông tin course hiển thị ra trong màn hình coursepayment
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="classId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("payment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseOrderDto>> GetCoursePaymentAsync(int courseId, int classId)
    {
        var result = await _courseService.GetCoursePaymentAsync(courseId, classId);
        return Ok(result);
    }

    /// <summary>
    /// Student click on start study button
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.StudentRole}")]
    [HttpPost("start-study")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<IActionResult> StartStudyCourseAsyn(StudentProgressRequest dto)
    {
        await _courseService.StartStudySectionAsync(dto);
        return Ok(new
        {
            Message = "Start study section successfully"
        });
    }

    /// <summary>
    /// Student click on mark complete button when finish document or video
    /// </summary>
    /// <param name="lessonId"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.StudentRole}")]
    [HttpPatch("mark-lesson-completed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<IActionResult> MartLessonCompletedAsync(int lessonId)
    {
        await _courseService.MarkLessonCompletedAsync(lessonId);
        return Ok(new
        {
            Message = "Mark lesson completed successfully"
        });
    }

    /// <summary>
    /// API INTERNAL TEST, Update Course (Status, Price)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="number">1. Update status, 2. Update Price</param>
    /// <returns></returns>
    [HttpPatch("update-status/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<IActionResult> UpdateToPendingStaus(int id, int number)
    {
        await _courseService.UpdateToPendingStatus(id, number);
        return Ok(new
        {
            Message = "Update to pending status completed successfully"
        });
    }

    /// <summary>
    /// Staff get list course moderation
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.StaffRole},{Constant.AdminRole}")]
    [HttpGet("moderation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<List<CourseModerationResponse>>> GetCourseModerationAsync()
    {
        var course = await _courseService.GetCourseModerationAsync();
        return Ok(course);
    }

    /// <summary>
    /// Student submit quiz
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.StudentRole}")]
    [HttpPost("quiz/submit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<QuizSubmitResponse>> SubmitQuizAsync(QuizSubmitRequest dto)
    {
        var result = await _quizService.StudentSubmitQuizAsync(dto);
        return Ok(result);
    }
    
    /// <summary>
    /// Filter teacher created courses.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="status"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpGet("teacher")]
    [Authorize(Roles = $"{Constant.TeacherRole}")]
    public async Task<ActionResult<PagingResponse<ManageFilterCourseDto>>> FilterTeacherCourseAsync(
        [FromQuery] string? name,
        [FromQuery] CourseStatus? status, [FromQuery] int? page, [FromQuery] int? size)
    {
        var result = await _courseService.FilterTeacherCourseAsync(name, status, page, size);
        return Ok(result);
    }

    /// <summary>
    /// Common user get ACTIVE course by id. The course just include outline (section, quiz, document, video)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("study/{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Application.Dtos.Response.Course.Study.StudyCourseDto>>
        GetActiveStudyCourseByIdAsync(
            [FromRoute] int id)
    {
        var result = await _courseService.GetActiveStudyCourseByIdAsync(id);
        return Ok(result);
    }
    
    /// <summary>
    /// Get study section by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// Need to authorize this api
    [HttpGet("study/section/{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<CommonStudySectionDto>> GetStudySectionByIdAsync([FromRoute] int id)
    {
        var result = await _courseService.GetActiveCourseStudySectionByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Teacher can get course detail that is currently active or created by him/her or the course that he/she teaches in a class
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("teacher/study/{id:int}")]
    [Authorize(Roles = $"{Constant.TeacherRole}")]
    public async Task<ActionResult<StudyCourseDto>> GetTeacherStudyCourseByIdAsync([FromRoute] int id)
    {
        var result = await _courseService.GetTeacherStudyCourseByIdAsync(id);
        return Ok(result);
    }
    
    /// <summary>
    /// Teacher get course section detail. Can just view section detail of course that is currently active or
    /// created by him/her or the course that he/she teaches in a class 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("teacher/section/{id:int}")]
    [Authorize(Roles = $"{Constant.TeacherRole}")]
    public async Task<ActionResult<CommonStudySectionDto>> GetTeacherSectionDetailByIdAsync([FromRoute] int id)
    {
        var result = await _courseService.GetTeacherSectionDetailByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Get lesson detail in teacher role
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("teacher/section/lesson/{id:int}")]
    [Authorize(Roles = $"{Constant.TeacherRole}")]
    public async Task<ActionResult<StudyLessonDto>> GetTeacherLessonDetailByIdAsync([FromRoute] int id)
    {
        var result = await _courseService.GetTeacherStudyLessonByIdAsync(id);
        return Ok(result);
    }
    
    /// <summary>
    /// Admin and staff can view course detail with all status in the system
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("admin/study/{id:int}")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.StaffRole}")]
    public async Task<ActionResult<StudyCourseDto>> GetStudyCourseByIdAsync([FromRoute] int id)
    {
        var result = await _courseService.GetStudyCourseByIdAsync(id);
        return Ok(result);
    }
    
    /// <summary>
    /// Admin/staff can view all section detail by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("admin/section/{id:int}")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.StaffRole}")]
    public async Task<ActionResult<CommonStudySectionDto>> GetSectionDetailByIdAsync([FromRoute] int id)
    {
        var result = await _courseService.GetSectionDetailByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Student get course by id to learn. It contains progress of student in this course
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("student/study/{id:int}")]
    [Authorize(Roles = $"{Constant.StudentRole}")]
    public async Task<ActionResult<StudyCourseDto>> GetStudentStudyCourseByIdAsync([FromRoute] int id)
    {
        var result = await _courseService.GetStudentStudyCourseByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Get section detail by id for student for study purpose.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("student/study/section/{id:int}")]
    [Authorize(Roles = $"{Constant.StudentRole}")]
    public async Task<ActionResult<CommonStudySectionDto>> GetStudentStudySectionDetailByIdAsync([FromRoute] int id)
    {
        var result = await _courseService.GetStudentSectionDetailByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Get lesson detail in student role. Student can just get lesson in their course.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("student/study/section/lesson/{id:int}")]
    [Authorize(Roles = $"{Constant.StudentRole}")]
    public async Task<ActionResult<StudyLessonDto>> GetStudentLessonDetailByIdAsync([FromRoute] int id)
    {
        var result = await _courseService.GetStudentStudyLessonByIdAsync(id);
        return Ok(result);
    }
}