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
        private IAuthenticationService _authentication;

        public OrderController(IOrderService order, IAuthenticationService authentication)
        {
            _order = order;
            _authentication = authentication;
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
            //Check if the account is activated or not or inactive
            _authentication.CheckAccountStatus();

            var result = await _order.CreateOrderAsync(dto);
            return Ok(new
            {
                OrderId = result
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
            //Check if the account is activated or not or inactive
            _authentication.CheckAccountStatus();
            
            var result = await _order.GetListOrderAsync(status);
            return Ok(result);
        }

        /// <summary>
        /// Get order detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.ParentRole},{Constant.StaffRole},{Constant.AdminRole},")]
        [HttpGet("detail/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        public async Task<ActionResult<OrderDetailResponse>> GetOrdersAsync(int id)
        {
            //Check if the account is activated or not or inactive
            _authentication.CheckAccountStatus();
            
            var result = await _order.GetOrderDetail(id);
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
            //Check if the account is activated or not or inactive
            _authentication.CheckAccountStatus();
            
            await _order.CanCelOrderAsync(dto);
            return Ok("Order cancellation request sent successfully");
        }

        /// <summary>
        /// Staff handle the parent's refund request, approving or refusing
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.StaffRole}")]
        [HttpPost("handle-cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> ApproveOrderCancellationAsync(OrderRefundRequest dto, ModerationStatus status)
        {
            //Check if the account is activated or not or inactive
            _authentication.CheckAccountStatus();
            
            await _order.HandleRefundRequest(dto, status);
            return Ok("Handling the request successfully");
        }
    }
}