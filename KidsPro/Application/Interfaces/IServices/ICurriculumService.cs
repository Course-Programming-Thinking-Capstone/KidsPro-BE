using Application.Dtos.Request.Curriculum;

namespace Application.Interfaces.IServices;

public interface ICurriculumService
{
    Task CreateAsync(CreateCurriculumDto dto);
}