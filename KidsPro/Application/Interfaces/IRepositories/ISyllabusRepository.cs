using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ISyllabusRepository:IBaseRepository<Syllabus>
{
    Task<int> GetNumberOfDraftSyllabusAsync(int teacherId);
}