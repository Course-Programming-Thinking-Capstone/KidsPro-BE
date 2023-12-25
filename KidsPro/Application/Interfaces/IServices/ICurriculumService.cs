using Application.Dtos.Request.Curriculum;

namespace Application.Interfaces.Services;

public interface ICurriculumService
{
    Task CreateAsync(CreateCurriculumDto dto);
}