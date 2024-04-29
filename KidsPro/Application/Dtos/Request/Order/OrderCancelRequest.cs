using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Order;

public class OrderCancelRequest
{
    public int OrderId { get; set; }
    [StringLength(250, ErrorMessage = "Name exceed 250 character.")]  public string? Reason { get; set; }
}