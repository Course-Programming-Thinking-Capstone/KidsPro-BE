using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Order : BaseEntity
{
    [MaxLength(50)] public string PaymentType { get; set; } = null!;

    [Precision(11, 2)] public decimal TotalPrice { get; set; }
    public int Quantity { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime Date { get; set; }

    [MaxLength(750)] public string? Note { get; set; }

    [Column(TypeName = "tinyint")] public OrderStatus Status { get; set; }

    public virtual GameVoucher? Voucher { get; set; }
    public int VoucherId { get; set; }

    public virtual Parent? Parent { get; set; }
    public int ParentId { get; set; }

    public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
}