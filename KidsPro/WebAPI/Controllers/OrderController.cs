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
        /// Create order
        /// </summary>
        /// <param name="dto">Payment Type: 1.ZaloPay, 2.Momo</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> CreateOrderAsync(OrderRequest dto)
        {
            var result=await _order.CreateOrderAsync(dto);
            return Ok(new
            {
                OrderId=result
            });
        }
        
        /// <summary>
        /// Get order list
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.ParentRole},{Constant.StaffRole},{Constant.AdminRole},")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        public async Task<ActionResult<List<OrderResponse>>> GetOrdersAsync(OrderStatus status)
        {
            var result=await _order.GetListOrderAsync(status);
            return Ok(result);
        }
        
        /// <summary>
        /// Get order detail
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.ParentRole},{Constant.StaffRole},{Constant.AdminRole},")]
        [HttpGet("detail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        public async Task<ActionResult<OrderDetailResponse>> GetOrdersAsync(int orderId)
        {
            var result=await _order.GetOrderDetail(orderId);
            return Ok(result);
        }
        
        /// <summary>
        /// Parent sent the request to staff for order cancel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.ParentRole}")]
        [HttpPost("request-cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> CanCelOrderAsync(OrderCancelRequest dto)
        {
            await _order.CanCelOrderAsync(dto);
            return Ok("Order cancellation request sent successfully");
        }
        
        /// <summary>
        /// Staff approve the request cancel order of the parent
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.StaffRole}")]
        [HttpPatch("approve-cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> ApproveOrderCancellationAsync(int orderId, int parentId)
        {
            await _order.ApproveOrderCancellationAsync(orderId,parentId);
            return Ok("Successfully approved the order cancellation request");
        }
    }
}
