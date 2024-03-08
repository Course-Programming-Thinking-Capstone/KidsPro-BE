using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.IRepositories;

public interface ISectionComponentNumberRepository:IBaseRepository<SectionComponentNumber>
{
    Task<SectionComponentNumber?> GetByTypeAsync(SectionComponentType type);
}