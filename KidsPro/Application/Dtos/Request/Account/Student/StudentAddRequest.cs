using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Validations;

namespace Application.Dtos.Request.Account.Student
{
    public record StudentAddRequest
    {
        [NameValidation] [Required] public string FullName { get; set; } = null!;

        [DataType(DataType.Date)]
        [StudentDayOfBirthValidation]
        [Required]
        public DateTime Birthday { get; set; }

        [Required] public int Gender { get; set; } = 1;
    };
}