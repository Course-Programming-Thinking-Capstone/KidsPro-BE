using System.ComponentModel.DataAnnotations;
using Application.Validations;
using Microsoft.EntityFrameworkCore;

namespace Application.Dtos.Request.Curriculum;

public record CreateCurriculumDto
{
    [Required] [StringLength(250)] public string Name { get; set; } = null!;

    public string? PictureUrl { get; set; }

    [StringLength(3000)] public string? Description { get; set; }

    public DateTime? OpenDate { get; set; }

    [StartSaleDateValidation] public DateTime? StartSaleDate { get; set; }

    public DateTime? EndSaleDate { get; set; }

    [Range(0, float.MaxValue)]
    [Precision(11, 2)]
    public decimal Price { get; set; }
}