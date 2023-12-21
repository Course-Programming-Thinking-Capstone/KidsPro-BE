using Application.Dtos.Request.Curriculum;
using Application.Interfaces.Services;

namespace Application.Services;

public class CurriculumService: ICurriculumService

{
    public IUnitOfWork UnitOfWork { get; set; }

    public CurriculumService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Task Create(CreateCurriculumDto dto)
    {
        throw new NotImplementedException();
    }
}