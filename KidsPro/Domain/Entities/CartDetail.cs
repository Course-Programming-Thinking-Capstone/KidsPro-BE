using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(CartId), nameof(CourseId))]
public class CartDetail
{
    public int CartId { get; set; }

    public int CourseId { get; set; }

    [Range(0, 255)]
    [Column(TypeName = "tinyint")]
    public int Quantity { get; set; }

    [Column(TypeName = "tinyint")] public CartDetailStatus Status { get; set; } = CartDetailStatus.Active;


    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    public virtual Cart Cart { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;
}