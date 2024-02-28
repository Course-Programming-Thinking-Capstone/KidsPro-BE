using Domain.Entities.Generic;

namespace Domain.Entities;

public class Game : BaseEntity
{
    public int RequiredLevel { get; set; }

    public virtual Section Section { get; set; } = null!;
    public int SectionId { get; set; }
    
    public virtual LevelType LevelType { get; set; } = null!;
    public int LevelTypeId { get; set; }
}