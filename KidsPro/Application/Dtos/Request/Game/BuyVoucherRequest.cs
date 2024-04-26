namespace Application.Dtos.Request.Game;

public class BuyVoucherRequest
{
    public int UserId { get; set; }
    public int Cost { get; set; }
    public int VoucherType { get; set; }
}