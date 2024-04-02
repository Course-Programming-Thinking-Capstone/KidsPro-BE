namespace Application.Dtos.Request.Order.Momo
{
    public class MomoPaymentRequest
    {
        public string partnerCode { get; set; } = string.Empty;

        public string requestId { get; set; } = string.Empty;
        public string ipnUrl { get; set; } =string.Empty;
        public long amount { get; set; } = 0;
        public string orderId { get; set; } = string.Empty;
        public string orderInfo { get; set; } =string.Empty;
        public string redirectUrl { get; set; } = string.Empty;
        public string requestType { get; set; } = "captureWallet";
        public string extraData { get; set; } = "eyJ1c2VybmFtZSI6ICJtb21vIn0=";
        public string signature { get; set; } = string.Empty;
        public string lang { get; set; } = "vi";
        public long transId { get; set; }
    }
}
