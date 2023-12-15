using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class OrderDetail
{
    [Key, Column(Order = 0)] public int OrderId { get; set; }

    [Key, Column(Order = 1)] public int CourseId { get; set; }

    [Range(0, 255)]
    [Column(TypeName = "tinyint")]
    public int Quantity { get; set; }

    [Range(0, float.MaxValue)] public decimal Price { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}