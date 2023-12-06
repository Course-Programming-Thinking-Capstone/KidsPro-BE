using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "tinyint")]
    public int Id { get; set; }

    [MaxLength(30)] public string Name { get; set; } = string.Empty;
}