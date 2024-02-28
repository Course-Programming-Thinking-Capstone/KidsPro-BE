using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class ItemOwned : BaseEntity
{
    [MaxLength(50)] public string DisplayName { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }

    public virtual GameItem GameItem { get; set; } = null!;
    public int GameItemId { get; set; }
}