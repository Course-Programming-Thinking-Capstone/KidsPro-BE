using Application.Dtos.Request.Order;
using Application.Dtos.Request.Order.Momo;
using Application.Dtos.Response.Order;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
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
            Course? _course;

            do
            {
                var _checkOrderCode = await _unitOfWork.OrderRepository.GetByOrderCode(HashingUtils.GenerateRandomString, false);
                getOrderCode = _checkOrderCode.Item1 != null ? null : _checkOrderCode.Item2;

                _course= await _unitOfWork.CourseRepository.CheckCourseExist(dto.CourseId);
                if (_course == null) throw new BadRequestException($"CourseId {dto.CourseId} doesn't exist");

            } while (getOrderCode == null);


            //Create Order
            var _order = new Order()
            {
                ParentId = dto.ParentId,
                VoucherId = _voucher != null ? dto.VoucherId : null,
                PaymentType = (PaymentType)dto.PaymentType,
                Quantity = dto.Quantity,
                TotalPrice = (_course.Price * dto.Quantity) - (_voucher != null ? _voucher.DiscountAmount : 0),
                Date = DateTime.UtcNow,
                Status = OrderStatus.Payment,
                OrderCode =getOrderCode,
                Note="course: "+_course.Name
            };

            //Create OrderDetail
            var _orderDetail = new OrderDetail()
            {
                Price = _course.Price,
                CourseId = dto.CourseId,
                Quantity=dto.Quantity,
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

        public async Task<bool> StatusToPendingAsync(int orderId, int parentId)
        {
            var _order = await _unitOfWork.OrderRepository.GetOrderPaymentAsync(parentId, orderId);
            if (_order != null)
            {
                _order.Status = OrderStatus.Pending;
                _unitOfWork.OrderRepository.Update(_order);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            throw new NotImplementException($"Update orderID {orderId} to status pending failed");
        }

    }
}
