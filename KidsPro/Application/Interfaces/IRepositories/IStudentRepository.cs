using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IStudentRepository:IBaseRepository<Student>
{
    Task<Student?> GameStudentLoginAsync(string email);
}