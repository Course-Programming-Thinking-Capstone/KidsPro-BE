using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ILevelTypeRepository:IBaseRepository<LevelType>
{
    public Task ForceAddRangeAsync(IEnumerable<LevelType> entities);
}