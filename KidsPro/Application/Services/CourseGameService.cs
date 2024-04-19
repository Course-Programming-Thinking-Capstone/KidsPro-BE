using Application.Dtos.Response.CourseGame;
using Application.Interfaces.IServices;
using Application.Mappers;

namespace Application.Services;

public class CourseGameService:ICourseGameService
{
    private IUnitOfWork _unitOfWork;
    private IAuthenticationService _authenticationService;

    public CourseGameService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
    }

    public async Task<List<CourseGameDto>> GetAvailableCourseGameAsync()
    {
        var entities = await _unitOfWork.CourseGameRepository.GetAvailableCourseGameAsync();

        return entities.Select(CourseGameMapper.CourseGameToCourseGameDto).ToList();
    }
}