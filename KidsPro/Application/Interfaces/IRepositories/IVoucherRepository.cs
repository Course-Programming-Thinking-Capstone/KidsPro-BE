using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.IRepositories
{
    public interface IVoucherRepository:IBaseRepository<GameVoucher>
    {
        Task<GameVoucher?> GetVoucherPaymentAsync(int id);
        Task<List<GameVoucher>?> GetListVoucher(int parentId, VoucherStatus status);
    }
}
