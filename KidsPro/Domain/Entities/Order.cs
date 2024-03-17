using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(OrderCode), IsUnique = true)]
public class Order : BaseEntity
{
    [Column(TypeName = "tinyint")] public PaymentType PaymentType { get; set; }

    [Precision(11, 2)] public decimal TotalPrice { get; set; }
    public int Quantity { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime Date { get; set; }

    [MaxLength(750)] public string? Note { get; set; }

    [Column(TypeName = "tinyint")] public OrderStatus Status { get; set; }

    [MaxLength(50)] public string OrderCode { get; set; } = null!;

    public virtual GameVoucher? Voucher { get; set; }
    public int? VoucherId { get; set; }

    public virtual Parent? Parent { get; set; }
    public int ParentId { get; set; }

    public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    public virtual Transaction? Transaction { get; set; }
}