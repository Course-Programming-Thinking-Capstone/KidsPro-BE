namespace Application.Dtos.Request.Order;

public class OrderCancelRequest
{
    public int OrderId { get; set; }
    public string? Reason { get; set; }
}