using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class TeacherProfile : BaseEntity
{
    [DataType(DataType.DateTime)]
    [Precision(2)]
    public DateTime FromDate { get; set; }

    [DataType(DataType.DateTime)]
    [Precision(2)]
    public DateTime ToDate { get; set; }

    [MaxLength(750)] public string? Description { get; set; }

    [StringLength(250)] public string? CertificatePicture { get; set; }

    public int TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;

    public virtual Staff AddedBy { get; set; } = null!;
    public virtual int AddedById { get; set; }
}