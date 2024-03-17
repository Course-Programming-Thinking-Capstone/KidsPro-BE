using Application.Dtos.Request.Order;
using Application.Dtos.Response.Order;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<OrderResponseDto>> CreateOrderAsync(OrderRequestDto dto)
        {
            var result=await _order.CreateOrderAsync(dto);
            return Ok(result);
        }
    }
}
