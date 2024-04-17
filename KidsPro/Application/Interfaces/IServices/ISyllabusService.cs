using Application.Dtos.Request.Syllabus;
using Application.Dtos.Response.Paging;
using Application.Dtos.Response.Syllabus;
using Domain.Enums;
using Domain.Enums.Status;

namespace Application.Interfaces.IServices;

public interface ISyllabusService
{
    Task<SyllabusDetailDto> CreateAsync(CreateSyllabusDto dto);
    Task<SyllabusDetailDto> GetByIdAsync(int id);

    Task<PagingResponse<FilterSyllabusDto>> FilterSyllabusAsync(string? name,
        SyllabusStatus? status,
        string? sortName,
        string? sortCreatedDate,
        int? page,
        int? size
    );
    
    Task<PagingResponse<FilterSyllabusDto>> FilterTeacherSyllabusAsync(string? name,
        string? sortName,
        string? sortCreatedDate,
        int? page,
        int? size
    );

    Task<int> GetNumberOfTeacherDraftSyllabusAsync();
}