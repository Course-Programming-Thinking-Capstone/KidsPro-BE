using Application.Configurations;
using Application.Dtos.Request.Curriculum;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class CurriculumService : ICurriculumService

{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IAuthenticationService _authenticationService;

    public CurriculumService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
    }

    public async Task CreateAsync(CreateCurriculumDto dto)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Role.Name != Constant.AdminRole
            && currentUser.Role.Name != Constant.TeacherRole
            && currentUser.Role.Name != Constant.StaffRole)
        {
            throw new ForbiddenException("Action forbidden.");
        }

        var entity = CurriculumMapper.CreateDtoToEntity(dto);

        throw new NotImplementedException();
    }


    private async Task<User> GetCurrentUser()
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