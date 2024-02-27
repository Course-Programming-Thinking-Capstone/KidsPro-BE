using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;

namespace Domain.Entities;

public class Payment : BaseEntity
{
    [MaxLength(250)] public string AccountNumber { get; set; } = null!;

    public bool IsDefault { get; set; }

    [Column(TypeName = "tinyint")]
    public PaymentType Type { get; set; }

    public virtual Parent Parent { get; set; } = null!;
    public virtual int ParentId { get; set; }
}