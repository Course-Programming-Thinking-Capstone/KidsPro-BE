using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IStudentRepository:IBaseRepository<Student>
{
    Task<Student?> GameStudentLoginAsync(string email);
    Task<List<Student>> GetStudents(int parentId,string role);
    Task<Student?> GetStudentInformation(int id);
    Task<Student?> WebStudentLoginAsync(string account);

}