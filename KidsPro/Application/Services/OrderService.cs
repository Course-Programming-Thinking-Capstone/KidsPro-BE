using Application.Dtos.Request.Order;
using Application.Dtos.Response.Order;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
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

        public async Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto dto)
        {
            var _voucher = await _unitOfWork.VoucherRepository.GetVoucher(dto.VoucherId);

            //Kiểm tra lấy order code, nếu đã tồn tại phải tạo ordercode mới
            string? getOrderCode = string.Empty;
            do
            {
                var _result = await _unitOfWork.OrderRepository.GetByOrderCode(GenerateOrderCode, false);
                getOrderCode = _result.Item1 != null ? null : _result.Item2;
            } while (getOrderCode != null);

            //Create Order
            var _order = new Order()
            {
                ParentId = dto.ParentId,
                VoucherId = _voucher != null ? dto.VoucherId : null,
                PaymentType = (PaymentType)dto.PaymentType,
                Quantity = dto.Quantity,
                TotalPrice = (dto.Price * dto.Quantity) - (_voucher != null ? _voucher.DiscountAmount : 0),
                Date = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderCode =getOrderCode,
            };

            //Create OrderDetail
            var _orderDetail = new OrderDetail()
            {
                Price = dto.Price,
                CourseId = dto.CourseId,
                Order = _order
            };

            //Create OrderDetail Sudent
            foreach(var x in dto.StudentId)
            {
                var _student = await _unitOfWork.StudentRepository.GetByIdAsync(x);
                if (_student != null)
                {
                    // Kiểm tra xem danh sách OrderDetails đã tồn tại hay chưa
                    if (_student.OrderDetails == null)
                        _student.OrderDetails = new List<OrderDetail>();

                    // Thêm _orderDetail vào danh sách hiện có
                    _student.OrderDetails.Add(_orderDetail);
                }
            }

            //Add data to database
            await _unitOfWork.OrderDetailRepository.AddAsync(_orderDetail);
            var result=await _unitOfWork.SaveChangeAsync();
            if (result < 0) 
                throw new NotImplementException("Create Order Failed");

            //Mapper
            return OrderMapper.OrderToOrderResponse(_order);
        }

        private string GenerateOrderCode(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }

       

    }
}
