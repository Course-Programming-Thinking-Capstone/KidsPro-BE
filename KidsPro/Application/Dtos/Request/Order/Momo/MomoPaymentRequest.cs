using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.Request.Order.Momo
{
    public class MomoPaymentRequest
    {
        public string partnerCode { get; set; } = "";

        public string requestId { get; set; } = string.Empty;
        public string ipnUrl { get; set; } = "";
        public long amount { get; set; } = 0;
        public string orderId { get; set; } = string.Empty;
        public string orderInfo { get; set; } = "KidsPro Service: Payment For Course";
        public string redirectUrl { get; set; } = "";
        public string requestType { get; set; } = "captureWallet";
        public string extraData { get; set; } = "eyJ1c2VybmFtZSI6ICJtb21vIn0=";
        public string signature { get; set; } = "";
        public string lang { get; set; } = "vi";
    }
}
