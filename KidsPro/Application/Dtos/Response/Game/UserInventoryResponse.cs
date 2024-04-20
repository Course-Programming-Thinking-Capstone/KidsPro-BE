namespace Application.Dtos.Response.Game;

public class UserInventoryResponse
{
    public int Quantity { get; set; }
    public GameItemResponse GameItem { get; set; } = null!;
}