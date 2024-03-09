using Domain.Entities.Generic;

namespace Domain.Entities;

public class Student : BaseEntity
{
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public int ParentId { get; set; }
    public virtual Parent Parent { get; set; } = null!;

    public virtual GameUserProfile GameUserProfile { get; set; } = null!;

    public virtual ICollection<OrderDetail>? OrderDetails { get; set; }

    public virtual ICollection<Certificate>? Certificates { get; set; }
}