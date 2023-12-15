using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(OrderId), nameof(CourseId))]
public class OrderDetail
{
    public int OrderId { get; set; }

    public int CourseId { get; set; }

    [Range(0, 255)]
    [Column(TypeName = "tinyint")]
    public int Quantity { get; set; }

    [Range(0, float.MaxValue)] [Precision(11,2)] public decimal Price { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}