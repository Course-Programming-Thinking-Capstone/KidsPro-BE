namespace Domain.Entities;

public class StudentClass
{
    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }
    public virtual Class Class { get; set; } = null!;
    public int ClassId { get; set; }
}