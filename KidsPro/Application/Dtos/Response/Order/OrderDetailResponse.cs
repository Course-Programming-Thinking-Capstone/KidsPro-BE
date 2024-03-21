using Application.Dtos.Response.Account.Student;

namespace Application.Dtos.Response.Order;

public class OrderDetailResponse
{
    public int OrderId { get; set; }
    public string? CourseName { get; set; }
    public string? PictureUrl { get; set; }
    public int QuantityPurchased { get; set; }
    public decimal? Discount { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
    public string? OrderCode { get; set; }
    public string? TransactionCode { get; set; }
    public string? OrderDate { get; set; }
    public string? PaymentType { get; set; }
    public string? Status { get; set; }
    public int NumberChildren { get; set; }
    public string? ParentEmail { get; set; }
    public string? ParentZalo { get; set; }
    public List<StudentOrderDetail>? Students { get; set; } = new List<StudentOrderDetail>();
    public string? ParentName { get; set; }
    public int ParentId { get; set; }
}