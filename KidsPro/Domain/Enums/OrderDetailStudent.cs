using Domain.Entities;

namespace Domain.Enums;

public class OrderDetailStudent
{
    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }

    public virtual OrderDetail OrderDetail { get; set; } = null!;
    public virtual int OrderDetailId { get; set; }
}