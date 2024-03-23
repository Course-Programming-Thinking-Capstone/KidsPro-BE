using Application.Dtos.Request.Syllabus;
using Application.Dtos.Response.Paging;
using Application.Dtos.Response.Syllabus;
using Domain.Enums;

namespace Application.Interfaces.IServices;

public interface ISyllabusService
{
    Task<SyllabusDetailDto> CreateAsync(CreateSyllabusDto dto);

    Task<PagingResponse<FilterSyllabusDto>> FilterSyllabusAsync(string? name,
        SyllabusStatus? status,
        string? sortName,
        string? sortCreatedDate,
        int? page,
        int? size
    );
}