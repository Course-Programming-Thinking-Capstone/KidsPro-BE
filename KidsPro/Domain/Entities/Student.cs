using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Student : BaseEntity
{
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public int ParentId { get; set; }
    public virtual Parent Parent { get; set; } = null!;

}