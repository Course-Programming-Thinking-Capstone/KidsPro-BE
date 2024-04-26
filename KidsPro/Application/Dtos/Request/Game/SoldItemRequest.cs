namespace Application.Dtos.Request.Game;

public class SoldItemRequest
{
    public int UserId { get; set; }
    public int SoldItemId { get; set; }
    public int Quantity { get; set; }
}