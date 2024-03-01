using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(SectionComponentType), IsUnique = true)]
public class SectionComponentNumber : BaseEntity
{
    [Column(TypeName = "tinyint")] public SectionComponentType SectionComponentType { get; set; }

    public int MaxNumber { get; set; }
}