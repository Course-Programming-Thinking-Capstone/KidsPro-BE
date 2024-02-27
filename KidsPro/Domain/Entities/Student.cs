using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Student : BaseEntity
{
    [Column(TypeName = "tinyint")] public Gender? Gender { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? DateOfBirth { get; set; }
    
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public int ParentId { get; set; }
    public virtual Parent Parent { get; set; } = null!;

}