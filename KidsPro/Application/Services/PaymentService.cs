using Application.Dtos.Request.Order.Momo;
using Application.Dtos.Response.Order.Momo;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Application.Dtos.Request.Order.ZaloPay;
using Application.Dtos.Response.Order.ZaloPay;
using WebAPI.Gateway.IConfig;

namespace Application.Services;

public class PaymentService : IPaymentService
{
    readonly IUnitOfWork _unitOfWork;
    private IAccountService _accountService;
    IMomoConfig _momoConfig;
    private IMomoService _momoService;

    public PaymentService(IUnitOfWork unitOfWork, IAccountService accountService, IMomoConfig momoConfig,
        IMomoService momoService)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
        _momoConfig = momoConfig;
        _momoService = momoService;
    }

    #region Transaction

    public async Task<int> CreateTransactionAsync(MomoResultRequest dto)
    {
        var orderId = GetIdMomoResponse(dto.orderId);
        //var parentId = GetIdMomoResponse(dto.requestId);

        var order = await _unitOfWork.OrderRepository.GetOrderByStatusAsync(orderId, OrderStatus.Process)
                    ?? throw new BadRequestException($"OrderId {orderId} not found, update to pending status failed");

        order.Status = OrderStatus.Pending;
        _unitOfWork.OrderRepository.Update(order);

        var transaction = new Transaction()
        {
            OrderId = orderId,
            CreatedDate = DateTime.UtcNow,
            TransactionCode = dto.transId,
            Amount = dto.amount,
            Status = TransactionStatus.Success
        };
        await _unitOfWork.TransactionRepository.AddAsync(transaction);
        var result = await _unitOfWork.SaveChangeAsync();
        if (result < 0) throw new NotImplementException("Add transaction failed");
        return orderId;
    }

    private async Task<Transaction> GetTransactionByOrderIdAsync(int orderId)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(orderId);
        return transaction ?? throw new NotFoundException($"OrderId: {orderId} not found in transaction");
    }

    public async Task UpdateTransStatusToRefunded(string momoOrderId)
    {
        var orderId = GetIdMomoResponse(momoOrderId);

        var transaction = await GetTransactionByOrderIdAsync(orderId);
        transaction.Status = TransactionStatus.Refunded;

        _unitOfWork.TransactionRepository.Update(transaction);
        await _unitOfWork.SaveChangeAsync();
    }

    #endregion

    #region Momo

    private int GetIdMomoResponse(string id)
    {
        Regex regex = new Regex("-(\\d+)");
        var macth = regex.Match(id);
        if (macth.Success) return Int32.Parse(macth.Groups[1].Value);
        return 0;
    }

    public async Task<MomoPaymentResponse> RequestMomoRefundAsync(int orderId)
    {
        var momoRequest = new MomoRefundRequest();
        //Get Transaction by momoOrderId
        var transaction = await GetTransactionByOrderIdAsync(orderId);

        // Lấy thông tin cho payment
        momoRequest.requestId = StringUtils.GenerateRandomNumber(4) + "-" + transaction.Order!.ParentId;
        momoRequest.orderId = StringUtils.GenerateRandomNumber(4) + "-" + transaction.OrderId;
        momoRequest.amount = (long)transaction.Amount;
        momoRequest.partnerCode = _momoConfig.PartnerCode;
        momoRequest.transId = long.Parse(transaction.TransactionCode!);
        momoRequest.description = "KidsPro refunds for order with OrderCode '" + transaction.Order!.OrderCode + "'";
        momoRequest.signature = _momoService.MakeSignatureMomoRefund
            (_momoConfig.AccessKey, _momoConfig.SecretKey, momoRequest);

        // Request Momo Refund
        return _momoService.SentRequestMomoRefund(_momoConfig.RefundUrl, momoRequest);
    }

    #endregion

    #region ZaloPay

    private Dictionary<string, string> GetContent(ZaloPaymentRequest zaloRequest)
    {
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        keyValuePairs.Add("app_id", zaloRequest.AppId.ToString());
        keyValuePairs.Add("app_user", zaloRequest.AppUser);
        keyValuePairs.Add("app_time", zaloRequest.AppTime.ToString());
        keyValuePairs.Add("amount", zaloRequest.Amount.ToString());
        keyValuePairs.Add("app_trans_id", zaloRequest.AppTransId);
        keyValuePairs.Add("bank_code", "zalopayapp");
        keyValuePairs.Add("embed_data", "{}");
        keyValuePairs.Add("item", "[]");
        keyValuePairs.Add("callback_url", "https://localhost:44317/swagger/index.html");
        keyValuePairs.Add("description", zaloRequest.Description);
        keyValuePairs.Add("mac", zaloRequest.Mac);

        return keyValuePairs;
    }

    public string? GetLinkGatewayZaloPay(string paymentUrl, ZaloPaymentRequest zaloRequest)
    {
        using HttpClient client = new HttpClient();
        var content = new FormUrlEncodedContent(GetContent(zaloRequest));
        var response = client.PostAsync(paymentUrl, content).Result;
        if (response.IsSuccessStatusCode)
        {
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var responeseData = JsonConvert.DeserializeObject<ZaloPaymentResponse>(responseContent);
            // return QRcode
            if (responeseData?.returnCode == 1)
                return responeseData.orderUrl;
            throw new NotImplementException($"Error Momo: {responeseData?.returnMessage}");
        }

        throw new NotImplementException($"Error Momo: {response.ReasonPhrase}");
    }

    public string MakeSignatureZaloPayment(string key, ZaloPaymentRequest zalo)
    {
        var data = zalo.AppId + "|" + zalo.AppTransId + "|" + zalo.AppUser + "|" + zalo.Amount + "|" + zalo.AppTime +
                   "|" + "{}" + "|" + "[]";

        return zalo.Mac = HashingUtils.HmacSha256(data, key);
    }

    #endregion
}