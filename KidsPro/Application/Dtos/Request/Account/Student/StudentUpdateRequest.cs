﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Validations;

namespace Application.Dtos.Request.Account.Student
{
    public class StudentUpdateRequest
    {
        public int Id { get; set; }
        [NameValidation] [Required] public string FullName { get; set; } = string.Empty;

        [StudentDayOfBirthValidation]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }


        [DataType(DataType.EmailAddress)] public string Email { get; set; } = string.Empty;

        [PasswordValidation] public string Password { get; set; } = string.Empty;

        public int Gender { get; set; }
    }
}