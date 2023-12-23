using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Role:BaseEntity
{
    [MaxLength(30)] public string Name { get; set; } = string.Empty;
}