using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ILessonRepository:IBaseRepository<Lesson>
{
    Task<bool> ExistBySectionIdAndOrder(int sectionId, int order);
}