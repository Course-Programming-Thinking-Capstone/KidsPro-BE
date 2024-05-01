using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.IRepositories;

public interface IStudentRepository:IBaseRepository<Student>
{
    Task<Student?> GameStudentLoginAsync(string account);
    Task<List<Student>> GetStudents(string role,int parentId=0);
    Task<Student?> GetStudentInformation(int id);
    Task<Student?> WebStudentLoginAsync(string account);
    Task<List<Student>> SearchStudent(string input, SearchType type);
    Task<List<Student>> GetStudentsByIds(List<int> ids);
    Task<Student?> GetStudentProgress(int id);
    Task<Student?> StudentGetStudentLessonAsync(int id);
    Task<Student?> CheckNameOverlapAsync(string? name);
}