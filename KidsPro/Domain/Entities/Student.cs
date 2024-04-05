using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(UserName), IsUnique = true)]
public class Student : BaseEntity
{
    public int AccountId { get; set; }

    public Account Account { get; set; } = null!;

    public int ParentId { get; set; }

    public virtual Parent Parent { get; set; } = null!;

    [MaxLength(15)] public string? UserName { get; set; } = null!;

    public virtual GameUserProfile GameUserProfile { get; set; } = null!;

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Certificate>? Certificates { get; set; }

    public virtual ICollection<StudentProgress>? StudentProgresses { get; set; }
    public virtual IEnumerable<Class> Classes { get; } = new List<Class>();
    public virtual ICollection<StudentQuiz> StudentQuizzes { get; set; } = new List<StudentQuiz>();
    
    
}