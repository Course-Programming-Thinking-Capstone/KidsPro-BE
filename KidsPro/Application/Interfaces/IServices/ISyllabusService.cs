using Application.Dtos.Request.Syllabus;
using Application.Dtos.Response.Syllabus;

namespace Application.Interfaces.IServices;

public interface ISyllabusService
{
    Task<SyllabusDetailDto> CreateAsync(CreateSyllabusDto dto);
}