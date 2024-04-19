using Application.Dtos.Response.CourseGame;

namespace Application.Interfaces.IServices;

public interface ICourseGameService
{
    Task<List<CourseGameDto>> GetAvailableCourseGameAsync();
}