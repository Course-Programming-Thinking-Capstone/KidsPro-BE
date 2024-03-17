using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class GameVoucher : BaseEntity
{
    public int ConvertedPoint { get; set; }

    [Precision(11, 2)] public decimal DiscountAmount { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? ExpiredDate { get; set; }

    [Column(TypeName = "tinyint")] public VoucherStatus Status { get; set; } = VoucherStatus.Valid;

    public virtual Order? Order { get; set; }
    
    public virtual Parent? Parent { get; set; }
    public int? ParentId { get; set; }
}