namespace Application.Dtos.Response.Order.Momo;

public class MomoPaymentResponse
{
    public string partnerCode { get; set; } = string.Empty;
    public string orderId { get; set; } = string.Empty;
    public string requestId { get; set; } = string.Empty;

    public long amount { get; set; }
    public long transId { get; set; }
    public int resultCode { get; set; }
    public string message { get; set; } = string.Empty;
    public long responseTime { get; set; }

    public string payUrl { get; set; } = string.Empty;
    public string qrCodeUrl { get; set; } = string.Empty;
}