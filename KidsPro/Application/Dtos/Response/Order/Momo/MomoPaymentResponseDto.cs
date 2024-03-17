using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response.Order.Momo;

public class MomoPaymentResponseDto
{
    public string partnerCode { get; set; } = string.Empty;
    public string amount { get; set; } = string.Empty;
    public string orderId { get; set; } = string.Empty;
    public string payUrl { get; set; } = string.Empty;
    public string resultCode { get; set; } = string.Empty;
    public string message { get; set; } = string.Empty;
    public string responseTime { get; set; } = string.Empty;
    public string qrCodeUrl { get; set; } = string.Empty;
}
