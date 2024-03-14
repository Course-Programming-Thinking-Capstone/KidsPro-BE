using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Request.Student
{
    public class StudentUpdateDto
    {
        public int Id {  get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateTime BirthDay { get; set; }


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int Gender { get; set; } 

    }
}
