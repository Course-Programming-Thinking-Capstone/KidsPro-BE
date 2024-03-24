using Domain.Entities.Generic;

namespace Domain.Entities;

public class StudentClass:BaseEntity
{
    public virtual Class Class { get; set; } = null!;
    public int ClassId { get; set; }

    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }
}