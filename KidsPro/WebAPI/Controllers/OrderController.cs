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
        /// API INTERNAL, MOBILE get order by status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.ParentRole}")]
        [HttpGet("mobile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        public async Task<ActionResult<List<OrderResponse>>> MobileGetOrdersAsync(OrderStatus status)
        {
            //Check if the account is activated or not or inactive
            _authentication.CheckAccountStatus();

            var result = await _order.MobileGetListOrderAsync(status);
            return Ok(result);
        }

        /// <summary>
        /// Create order
        /// </summary>
        /// <param name="dto">Process Type: 1.ZaloPay, 2.Momo</param>
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
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.ParentRole},{Constant.StaffRole},{Constant.AdminRole},")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        public async Task<ActionResult<PagingOrderResponse>> GetOrdersAsync(int? pageSize, int? pageNumber,
            OrderStatus status)
        {
            //Check if the account is activated or not or inactive
            _authentication.CheckAccountStatus();

            var result = await _order.GetListOrderAsync(status, pageSize, pageNumber);
            return Ok(result);
        }

        /// <summary>
        /// Get order detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
     //   [Authorize(Roles = $"{Constant.ParentRole},{Constant.StaffRole},{Constant.AdminRole},")]
        [HttpGet("detail/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        public async Task<ActionResult<OrderDetailResponse>> GetOrdersDetailAsync(int id)
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

            await _order.ParentCanCelOrderAsync(dto);
            return Ok("Order cancellation request sent successfully");
        }

        /// <summary>
        /// Staff handle the parent's refund request, approving or refusing
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.StaffRole},{Constant.AdminRole}")]
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

        /// <summary>
        /// Staff click on confirm button, order update from pending status to success status
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.StaffRole},{Constant.AdminRole}")]
        [HttpPatch("confirm/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> ConfirmOrderAsync(int id)
        {
            //Check if the account is activated or not or inactive
            _authentication.CheckAccountStatus();

            await _order.ConfirmOrderAsync(id);
            
            return Ok("Successfully update to success status");
        }

        /// <summary>
        /// Admin and staff search order by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{Constant.StaffRole},{Constant.AdminRole}")]
        [HttpGet("search/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
        public async Task<IActionResult> SearchOrderByCodeAsync(string code)
        {
            //Check if the account is activated or not or inactive
            _authentication.CheckAccountStatus();

            var result = await _order.SearchOrderByCodeAsync(code);
            return Ok(result);
        }
    }
}