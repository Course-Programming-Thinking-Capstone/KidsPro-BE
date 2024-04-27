using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Parent;
using Application.Dtos.Response.Account.Student;
using Application.ErrorHandlers;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.IServices;

    public interface IParentsService
    {
        Task<StudentResponse> AddStudentAsync(StudentAddRequest request);

        Task<ParentOrderResponse> GetParentEmail();

        Task<List<GameVoucher>?> GetListVoucherAsync(VoucherStatus status);

    }

