using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.CourseModeration;
using Application.Dtos.Response.Course.FilterCourse;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos.Response.Paging;
using Domain.Enums;
using WebAPI.Controllers;

namespace UnitTest
{
    [TestFixture]
    public class CoursesControllerTests
    {
        private Mock<ICourseService> _mockCourseService;
        private Mock<IQuizService> _mockQuizService;
        private Mock<IProgressService> _mockProgressService;
        private CoursesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockCourseService = new Mock<ICourseService>();
            _mockQuizService = new Mock<IQuizService>();
            _mockProgressService = new Mock<IProgressService>();
            _controller = new CoursesController(_mockCourseService.Object, _mockQuizService.Object,
                _mockProgressService.Object);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCourse_WhenIdIsValid()
        {
            // Arrange
            int courseId = 1;
            var expectedCourse = new CourseDto { Id = courseId };

            _mockCourseService.Setup(s => s.GetByIdAsync(courseId, null)).ReturnsAsync(expectedCourse);

            // Act
            var result = await _controller.GetByIdAsync(courseId, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.AreEqual(expectedCourse, okResult.Value);
        }

        [Test]
        public async Task FilterCourseAsync_ReturnsFilteredCourses()
        {
            // Arrange
            var expectedCourses = new PagingResponse<FilterCourseDto>();

            _mockCourseService.Setup(s => s.FilterCourseAsync(null, null, null, null, null, null, null, null))
                .ReturnsAsync(expectedCourses);

            // Act
            var result = await _controller.FilterCourseAsync(null, null, null, null, null, null, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.AreEqual(expectedCourses, okResult.Value);
        }

        [Test]
        public async Task CreateCourseAsync_ReturnsCreatedCourse_WhenRequestIsValid()
        {
            // Arrange
            var requestDto = new CreateCourseDto();
            var expectedCourse = new CourseDto { Id = 1 };

            _mockCourseService.Setup(s => s.CreateCourseAsync(requestDto)).ReturnsAsync(expectedCourse);

            // Act
            var result = await _controller.CreateCourseAsync(requestDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CreatedResult>(result.Result);
        }

        [Test]
        public async Task GetCourseModerationAsync_ReturnsCourseModerationList()
        {
            // Arrange
            var expectedCourseModerationList = new List<CourseModerationResponse>();

            _mockCourseService.Setup(s => s.GetCourseModerationAsync()).ReturnsAsync(expectedCourseModerationList);

            // Act
            var result = await _controller.GetCourseModerationAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.AreEqual(expectedCourseModerationList, okResult.Value);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCourse_WhenIdExists()
        {
            // Arrange
            int courseId = 1;
            var expectedCourse = new CourseDto { Id = courseId };

            _mockCourseService.Setup(s => s.GetByIdAsync(courseId, null)).ReturnsAsync(expectedCourse);

            // Act
            var result = await _controller.GetByIdAsync(courseId, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.AreEqual(expectedCourse, okResult.Value);
        }

        [Test]
        public async Task ApproveCourseAsync_ReturnsOkResult_WhenCourseIsApproved()
        {
            // Arrange
            int courseId = 1;
            var requestDto = new AcceptCourseDto();

            // Act
            var result = await _controller.ApproveCourseAsync(courseId, requestDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task DenyCourseAsync_ReturnsOkResult_WhenCourseIsDenied()
        {
            // Arrange
            int courseId = 1;
            string reason = "Course content does not meet standards";

            // Act
            var result = await _controller.DenyCourseAsync(courseId, reason);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task UpdateCoursePictureAsync_ReturnsOkResult_WhenPictureIsUpdated()
        {
            // Arrange
            int courseId = 1;
            var file = new FormFile(null, 0, 0, "file", "test.jpg");

            // Act
            var result = await _controller.UpdateCoursePictureAsync(courseId, file);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.AreEqual(null, okResult.Value);
        }

        [Test]
        public async Task FilterTeacherCourseAsync_ReturnsFilteredCourses_WhenRequestIsValid()
        {
            // Arrange
            string name = "Math";
            CourseStatus? status = CourseStatus.Active;
            int page = 1;
            int size = 10;
            var expectedResponse = new PagingResponse<ManageFilterCourseDto>();

            _mockCourseService.Setup(s => s.FilterTeacherCourseAsync(name, status, page, size))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.FilterTeacherCourseAsync(name, status, page, size);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.AreEqual(expectedResponse, okResult.Value);
        }
    }
}