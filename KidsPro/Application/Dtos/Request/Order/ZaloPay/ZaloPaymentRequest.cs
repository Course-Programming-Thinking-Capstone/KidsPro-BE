namespace Application.Dtos.Request.Order.ZaloPay;

public class ZaloPaymentRequest
{
    public int AppId { get; set; }
    public string? AppUser { get; set; }
    public string? AppTransId { get; set; }
    public long AppTime { get; set; }
    public long Amount { get; set; }
    public string? Description { get; set; }  
    public string? BankCode { get; set; } 
    public string? Mac { get; set; } 
    public string? EmbedData { get; set; } 
}