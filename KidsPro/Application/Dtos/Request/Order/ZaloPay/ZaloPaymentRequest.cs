namespace Application.Dtos.Request.Order.ZaloPay;

public class ZaloPaymentRequest
{
    public int AppId { get; set; }
    public string AppUser { get; set; } = string.Empty;
    public long AppTime { get; set; }
    public long Amount { get; set; }
    public string AppTransId { get; set; } = string.Empty;
    public string ReturnUrl { get; }
   // public string EmbedData { get; set; } = string.Empty;
   // public string[] Item { get; set; }=
    public string Mac { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}