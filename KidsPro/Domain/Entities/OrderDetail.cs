using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(OrderId), nameof(StudentId))]
public class OrderDetail
{
    public virtual Order Order { get; set; } = null!;
    public int OrderId { get; set; }

    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }
}