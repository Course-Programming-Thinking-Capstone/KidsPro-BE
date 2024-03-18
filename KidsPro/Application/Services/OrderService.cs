using Application.Dtos.Request.Order;
using Application.Dtos.Response.Order;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class OrderService:IOrderService
    {
        readonly IUnitOfWork _unitOfWork;
        private IParentsService _parentsService;
        public OrderService(IUnitOfWork unitOfWork, IParentsService parentsService)
        {
            _unitOfWork = unitOfWork;
            _parentsService = parentsService;
        }

        public async Task<OrderPaymentResponseDto> CreateOrderAsync(OrderRequestDto dto)
        {
            var voucher = await _unitOfWork.VoucherRepository.GetVoucher(dto.VoucherId);
            //Kiểm tra lấy order code, nếu đã tồn tại phải tạo ordercode mới
            string? getOrderCode;
            Course? course;
            do
            {
                var checkOrderCode = await _unitOfWork.OrderRepository.GetByOrderCode(HashingUtils.GenerateRandomString, false);
                getOrderCode = checkOrderCode.Item1 != null ? null : checkOrderCode.Item2;
                course= await _unitOfWork.CourseRepository.CheckCourseExist(dto.CourseId);
                if (course == null) throw new BadRequestException($"CourseId {dto.CourseId} doesn't exist");

            } while (getOrderCode == null);

            var parent = await _parentsService.GetInformationParentCurrentAsync();

            //Create Order
            var order = new Order()
            {
                ParentId = parent?.Id ?? throw new UnauthorizedException("ParentId doesn't exist"),
                VoucherId = voucher != null ? dto.VoucherId : null,
                PaymentType = (PaymentType)dto.PaymentType,
                Quantity = dto.Quantity,
                TotalPrice = (course.Price * dto.Quantity) - (voucher?.DiscountAmount ?? 0),
                Date = DateTime.UtcNow,
                Status = OrderStatus.Payment,
                OrderCode =getOrderCode,
                Note="course: "+course.Name
            };

            //Create OrderDetail
            var orderDetail = new OrderDetail()
            {
                Price = course.Price,
                CourseId = dto.CourseId,
                Quantity=dto.Quantity,
                Order = order
            };

            //Create OrderDetail Student
            foreach(var x in dto.StudentId)
            {
                var student = await _unitOfWork.StudentRepository.GetByIdAsync(x);
                if (student != null)
                {
                    // Kiểm tra xem danh sách OrderDetails đã tồn tại hay chưa
                    if (student.OrderDetails == null)
                        student.OrderDetails = new List<OrderDetail>();

                    // Thêm _orderDetail vào danh sách hiện có
                    student.OrderDetails.Add(orderDetail);
                }
            }

            //Add data to database
            await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);
            var result=await _unitOfWork.SaveChangeAsync();
            if (result < 0) 
                throw new NotImplementException("Create Order Failed");
            //Mapper
            return OrderMapper.OrderToOrderPaymentResponse(order);
        }

        public async Task<bool> StatusToPendingAsync(int orderId, int parentId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderPaymentAsync(parentId, orderId);
            if (order != null)
            {
                order.Status = OrderStatus.Pending;
                _unitOfWork.OrderRepository.Update(order);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            throw new NotImplementException($"Update orderID {orderId} to status pending failed");
        }

        public async Task<List<OrderResponseDto>> GetListOrderAsync(OrderStatus status)
        {
            var parent = await _parentsService.GetInformationParentCurrentAsync();
            if (parent != null)
            {
                var orders = await _unitOfWork.OrderRepository.GetListOrderAsync(status, parent.Id);
                return OrderMapper.ParentShowOrder(orders!);
            }

            throw new UnauthorizedException("ParentId doesn't exist");
        }
        
    }
}
