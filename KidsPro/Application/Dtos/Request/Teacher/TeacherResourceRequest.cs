using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Application.Dtos.Request.Teacher
{
    public class TeacherResourceRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        [MaxLength(250)] public string ResourceUrl { get; set; } = string.Empty;
        
        [Column(TypeName = "tinyint")] public TeacherResourceType Type { get; set; }

        [Required]
        public int TeacherId { get; set; }

    }
}
