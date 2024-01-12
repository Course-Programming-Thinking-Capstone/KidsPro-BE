using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.Request.Teacher
{
    public class TeacherProfileRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [Required] public int TeacherId { get; set; }
    }
}
