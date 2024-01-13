using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Request.Teacher
{
    public class TeacherRequest
    {
        public int Id { get; set; }
        [MaxLength(150)] public string? Field { get; set; }

        [MaxLength(3000)] public string? Description { get; set; }
    }
}
