using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid UserId { get; set; }

    [StringLength(750)] public string? Note { get; set; }

    [Column(TypeName = "tinyint")] public TransactionType? Type { get; set; }

    [Column(TypeName = "tinyint")] public PaymentType? Method { get; set; }

    [StringLength(250)] public string? SourceAccount { get; set; }

    [StringLength(250)] public string? DestinationAccount { get; set; }

    [StringLength(250)] public string? TransactionCode { get; set; }

    [StringLength(100)] public string? Code { get; set; }

    [Column(TypeName = "tinyint")] public TransactionStatus Status { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? ProcessedDate { get; set; }

    public Guid? StaffId { get; set; }

    public User? Staff { get; set; }

    public virtual User User { get; set; } = null!;
}