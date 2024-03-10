using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IPositionTypeRepository:IBaseRepository<PositionType>
{
    public Task ForceAddRangeAsync(IEnumerable<PositionType> entities);
}