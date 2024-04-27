namespace Application.Dtos.Response.Order;

public class PagingOrderResponse
{
    public int TotalPage { get; set; }
    public int TotalRecord { get; set; }
    public List<OrderResponse> Classes { get; set; }= new List<OrderResponse>();
}