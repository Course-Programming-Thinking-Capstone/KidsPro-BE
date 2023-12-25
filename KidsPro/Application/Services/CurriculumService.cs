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
    public IUnitOfWork UnitOfWork { get; set; }

    public IAuthenticationService AuthenticationService { get; set; }

    public CurriculumService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
    {
        UnitOfWork = unitOfWork;
        AuthenticationService = authenticationService;
    }

    public async Task CreateAsync(CreateCurriculumDto dto)
    {
        var currentUser = await GetCurrentUser() ??
                          throw new UnauthorizedException("User does not exist or has been blocked.");
        if (currentUser.Role.Name != Constant.ADMIN_ROLE
            && currentUser.Role.Name != Constant.TEACHER_ROLE
            && currentUser.Role.Name != Constant.STAFF_ROLE)
        {
            throw new ForbiddenException("Action forbidden.");
        }

        var entity = CurriculumMapper.CreateDtoToEntity(dto);

        throw new NotImplementedException();
    }


    private async Task<User?> GetCurrentUser()
    {
        AuthenticationService.GetCurrentUserInformation(out var currentUserId, out var roleName);

        return await UnitOfWork.UserRepository
            .GetAsync(
                filter: u => u.Id == currentUserId && u.Status == UserStatus.Active,
                orderBy: null,
                includeProperties: $"{nameof(User.Role)}",
                disableTracking: true
            )
            .ContinueWith(t =>
                t.Result.Any() ? t.Result.FirstOrDefault() : throw new UnauthorizedException("Invalid token."));
    }
}