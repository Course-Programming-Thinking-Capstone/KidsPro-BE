namespace Application.Dtos.Request.Order.Momo;

public class MomoRefundRequest
{
    public string partnerCode { get; set; } = string.Empty;
    public string orderId { get; set; } = string.Empty;

    public string requestId { get; set; } = string.Empty;
    public long amount { get; set; } = 0;
    public long transId { get; set; }
    public string lang { get; set; } = "vi";
    public string description { get; set; } = "I want refund this order";
    public string signature { get; set; } = string.Empty;
}