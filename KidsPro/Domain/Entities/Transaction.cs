using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Transaction : BaseEntity
{
    [MaxLength(750)] public string? Note { get; set; }

    public TransactionType Type { get; set; }

    [MaxLength(50)] public string PaymentType { get; set; } = null!;

    [MaxLength(250)] public string? SourceAccount { get; set; }
    [MaxLength(250)] public string? DestinationAccount { get; set; }

    public decimal Amount { get; set; }

    [MaxLength(250)] public string? TransactionCode { get; set; }

    [MaxLength(100)] public string? VerificationCode { get; set; }

    [Column(TypeName = "tinyint")] public TransactionStatus Status { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? ProcessedDate { get; set; }

    public virtual Staff? Staff { get; set; }
    public int? StaffId { get; set; }
}