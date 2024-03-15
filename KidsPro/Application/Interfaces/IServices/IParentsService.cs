using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Parent;
using Application.Dtos.Response.Account.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IParentsService
    {
        Task<StudentDto> AddStudentAsync(StudentAddDto request);

        Task<List<StudentDto>> GetStudentsAsync(int parentId);

        Task<StudentDetailDto> GetDetailStudentAsync(int studentId);
        Task UpdateStudentAsync(StudentUpdateDto dto);

        ParentOrderDto GetEmailZalo(int parentId);
    }
}
