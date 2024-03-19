using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.Request.Account.Student
{
    public class CertificatesRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int StudentId { get; set; }

        public int CourseId { get; set; }

        [MaxLength(250)] public string ResourceUrl { get; set; } = string.Empty;


        [StringLength(750)] public string Description { get; set; } = string.Empty;
    }
}
