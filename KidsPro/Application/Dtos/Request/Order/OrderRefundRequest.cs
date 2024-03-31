namespace Application.Dtos.Request.Order;

public class OrderRefundRequest
{
    public int ParentId { get; set; }
    public int OrderId { get; set; }
    public string? ReasonRefuse { get; set; }
}