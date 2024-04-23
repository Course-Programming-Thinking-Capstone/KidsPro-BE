using System.Text;
using Application.Dtos.Request.Order.Momo;
using Application.Dtos.Response.Order.Momo;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.Services;

public class MomoService:IMomoService
{
    public MomoPaymentResponse SentRequestMomoRefund(string paymentUrl, MomoRefundRequest momoRequest)
    {
        using HttpClient client = new HttpClient();
        var requestData = JsonConvert.SerializeObject(momoRequest, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
        });
        var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

        var createPaymentLink = client.PostAsync(paymentUrl, requestContent).Result;
        if (createPaymentLink.IsSuccessStatusCode)
        {
            var responseContent = createPaymentLink.Content.ReadAsStringAsync().Result;
            var responeseData = JsonConvert.DeserializeObject<MomoPaymentResponse>(responseContent);
            // return QRcode
            if (responeseData?.resultCode == 0)
                return responeseData;
            throw new NotImplementException($"Error Momo: {responeseData?.message}");
        }

        throw new NotImplementException($"Error Momo: {createPaymentLink.ReasonPhrase}");
    }
    
    public (string?, string?) GetLinkMomoGateway(string paymentUrl, MomoPaymentRequest momoRequest)
    {
        using HttpClient client = new HttpClient();
        var requestData = JsonConvert.SerializeObject(momoRequest, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
        });
        var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

        var createPaymentLink = client.PostAsync(paymentUrl, requestContent).Result;
        if (createPaymentLink.IsSuccessStatusCode)
        {
            var responseContent = createPaymentLink.Content.ReadAsStringAsync().Result;
            var responeseData = JsonConvert.DeserializeObject<MomoPaymentResponse>(responseContent);
            // return QRcode
            if (responeseData?.resultCode == 0)
                return (responeseData.payUrl, responeseData.qrCodeUrl);
            throw new NotImplementException($"Error Momo: {responeseData?.message}");
        }

        throw new NotImplementException($"Error Momo: {createPaymentLink.ReasonPhrase}");
    }

    public string MakeSignatureMomoPayment(string accessKey, string secretKey, MomoPaymentRequest momo)
    {
        var rawHash = "accessKey=" + accessKey +
                      "&amount=" + momo.amount + "&extraData=" + momo.extraData +
                      "&ipnUrl=" + momo.ipnUrl + "&orderId=" + momo.orderId +
                      "&orderInfo=" + momo.orderInfo + "&partnerCode=" + momo.partnerCode +
                      "&redirectUrl=" + momo.redirectUrl + "&requestId=" + momo.requestId + "&requestType=" +
                      momo.requestType;
        return momo.signature = HashingUtils.HmacSha256(rawHash, secretKey);
    }

    public string MakeSignatureMomoRefund(string accessKey, string secretKey, MomoRefundRequest momo)
    {
        var rawHash = "accessKey=" + accessKey +
                      "&amount=" + momo.amount + "&description=" + momo.description +
                      "&orderId=" + momo.orderId + "&partnerCode=" + momo.partnerCode +
                      "&requestId=" + momo.requestId + "&transId=" + momo.transId;
        return momo.signature = HashingUtils.HmacSha256(rawHash, secretKey);
    }
    
}