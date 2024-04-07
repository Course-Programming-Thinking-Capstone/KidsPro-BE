using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class OrderDetail : BaseEntity
{
    [Precision(11, 2)] public decimal Price { get; set; }

    public int Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;
    public int OrderId { get; set; }

    public ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual Course Course { get; set; } = null!;
    public int CourseId { get; set; }
    
    public virtual Class Class { get; set; } = null!;
    public int ClassId { get; set; }
}