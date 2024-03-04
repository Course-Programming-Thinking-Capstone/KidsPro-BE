using Application.Dtos.Response.Course;
using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface ISectionRepository:IBaseRepository<Section>
{
    Task<bool> ExistByOrderAsync(int courseId, int order);
}