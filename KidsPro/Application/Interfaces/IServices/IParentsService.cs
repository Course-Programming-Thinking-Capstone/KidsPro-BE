using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Parent;
using Application.Dtos.Response.Account.Student;
using Application.ErrorHandlers;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.IServices;

    public interface IParentsService
    {
        Task<StudentResponseDto> AddStudentAsync(StudentAddRequestDto request);

        Task<List<StudentResponseDto>> GetStudentsAsync();

        Task<StudentDetailResponseDto> GetDetailStudentAsync(int studentId);
        Task UpdateStudentAsync(StudentUpdateRequestDto dto);

        Task<ParentOrderResponseDto> GetEmailZalo();

        Task<Parent?> GetInformationParentCurrentAsync();

        Task<List<GameVoucher>?> GetListVoucherAsync(VoucherStatus status);

    }

