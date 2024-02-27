using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class GameUserProfile:BaseEntity
{
    [MaxLength(50)]
    public string DisplayName { get; set; } = null!;
    public int Coin { get; set; }
    public int Gem { get; set; }
    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }
}