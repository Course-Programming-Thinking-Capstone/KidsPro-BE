namespace Domain.Enums;

public enum OrderStatus
{
    Payment = 0,
    Pending = 1,
    Success = 2,
    RequestRefund=3,
    Refunded = 4
}