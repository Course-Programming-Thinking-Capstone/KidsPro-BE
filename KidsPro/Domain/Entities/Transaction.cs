using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;


public class Transaction : BaseEntity
{

    [Precision(11, 2)] public decimal Amount { get; set; }

    [MaxLength(250)] public string? TransactionCode { get; set; }

    [Column(TypeName = "tinyint")] public TransactionStatus Status { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; }

    public virtual Order? Order { get; set; }
    public int? OrderId { get; set; }
}