using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Order;

public class OrderRefundRequest
{
    public int ParentId { get; set; }
    public int OrderId { get; set; }
    [StringLength(250, ErrorMessage = "Name exceed 250 character.")]   public string? ReasonRefuse { get; set; }
}