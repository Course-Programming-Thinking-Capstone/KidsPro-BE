using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Request.Authentication
{
    public record StudentRequest
    {
        [Required] public int ParentId { get; set; }
        [Required] public string FullName { get; set; } = null!;

        [Required] public DateTime Birthday { get; set; } 
        [Required] public int Gender { get; set; } 
    };
}

