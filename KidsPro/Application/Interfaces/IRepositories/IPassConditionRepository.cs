using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IPassConditionRepository:IBaseRepository<PassCondition>
{
    Task<PassCondition?> GetByPassRatioAsync(int passRatio, bool disableTracking = false);
}