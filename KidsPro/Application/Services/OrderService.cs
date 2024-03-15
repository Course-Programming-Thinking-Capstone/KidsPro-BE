using Application.Dtos.Request.Order;
using Application.Interfaces.IServices;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService:IOrderService
    {
        IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task CreateOrderAsync(OrderDto dto)
        //{
        //    var _voucher = await _unitOfWork.VoucherRepository.GetVoucher(dto.VoucherId);

        //    var _order = new Order()
        //    {
        //        ParentId = dto.ParentId,
        //       // VoucherId = _voucher != null ?? dto.VoucherId ? null,
        //      //  PaymentType=(PaymentType)dto.PaymentType,

        //    }
        //}

    }
}
