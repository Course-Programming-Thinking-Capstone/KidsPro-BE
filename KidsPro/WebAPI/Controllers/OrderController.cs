using Application.Configurations;
using Application.Dtos.Request.Order;
using Application.Dtos.Response.Order;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IOrderService _order;

        public OrderController(IOrderService order)
        {
            _order = order;
        }

        /// <summary>
        /// Tạo order và order detail
        /// </summary>
        /// <param name="dto">Payment Type: 1.ZaloPay, 2.Momo</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(ErrorDetail))]
        public async Task<ActionResult<OrderPaymentResponseDto>> CreateOrderAsync(OrderRequestDto dto)
        {
            var result=await _order.CreateOrderAsync(dto);
            return Ok(result);
        }
        
        /// <summary>
        /// Get order list by order status, Role Parent
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.ParentRole}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        public async Task<ActionResult<List<OrderResponseDto>>> GetOrdersAsync(OrderStatus status)
        {
            var result=await _order.GetListOrderAsync(status);
            return Ok(result);
        }
    }
}
