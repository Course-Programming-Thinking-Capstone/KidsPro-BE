using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Voucher : BaseEntity
{
    [MaxLength(20)] public string VoucherCode { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime ExpirationDate { get; set; }

    public int? UsageLimit { get; set; }

    public int NumberOfUsed { get; set; }

    [Precision(11, 2)] public decimal DiscountAmount { get; set; }

    [Column(TypeName = "tinyint")] public DiscountType DiscountType { get; set; }

    [Column(TypeName = "tinyint")] public VoucherStatus Status { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? ModifiedDate { get; set; }

    public virtual Account CreatedBy { get; set; } = null!;

    public int CreatedById { get; set; }
}