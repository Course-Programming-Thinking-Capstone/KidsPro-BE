using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class GameItem:BaseEntity
{
    public int GameId { get; set; }

    [MaxLength(50)]
    public string ItemName { get; set; } = null!;

    [MaxLength(500)]
    public string Details { get; set; } = null!;
    
    [MaxLength(250)]
    public string SpritesUrl { get; set; } = null!;
}