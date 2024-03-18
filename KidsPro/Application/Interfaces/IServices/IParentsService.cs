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
        Task<StudentResponseDto> AddStudentAsync(StudentAddRequestDto request);

        Task<List<StudentResponseDto>> GetStudentsAsync();

        Task<StudentDetailResponseDto> GetDetailStudentAsync(int studentId);
        Task UpdateStudentAsync(StudentUpdateRequestDto dto);

        Task<ParentOrderResponseDto> GetEmailZalo();
    }
}
