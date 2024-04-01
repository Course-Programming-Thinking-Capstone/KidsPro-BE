namespace Application.Dtos.Response.Order.ZaloPay;

public class ZaloPaymentResponse
{
    public int returnCode { get; set; }
    public string? returnMessage { get; set; }
    public string? orderUrl { get; set; }
}