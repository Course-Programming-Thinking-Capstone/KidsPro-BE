using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class OrderDetail : BaseEntity
{
    [Precision(11, 2)] public decimal Price { get; set; }

    public int Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;
    public int OrderId { get; set; }

    public virtual ICollection<Student> Students { get; set; } = null!;
    
    public virtual Course Course { get; set; } = null!;
    public int CourseId { get; set; }
}