namespace Application.Dtos.Response.Order;

public class PagingOrderResponse
{
    public int TotalPage { get; set; }
    public int TotalRecords { get; set; }
    public List<OrderResponse> Order { get; set; }= new List<OrderResponse>();
}