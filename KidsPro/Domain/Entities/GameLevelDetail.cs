using Domain.Entities.Generic;

namespace Domain.Entities;

public class GameLevelDetail : BaseEntity
{
    public int VPosition { get; set; }

    public virtual GameLevel GameLevel { get; set; } = null!;
    public int GameLevelId { get; set; }

    public virtual PositionType PositionType { get; set; } = null!;
    public int PositionTypeId { get; set; }
}