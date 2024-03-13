using Application.Dtos.Request.Student;
using Application.Dtos.Response.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IParentsService
    {
        Task<StudentDto> AddStudent(StudentAddDto request);

        Task<List<StudentDto>> GetStudents(int parentId);

        Task<StudentDetailDto> GetDetailStudent(int studentId);
        Task UpdateStudent(StudentUpdateDto dto);
    }
}
