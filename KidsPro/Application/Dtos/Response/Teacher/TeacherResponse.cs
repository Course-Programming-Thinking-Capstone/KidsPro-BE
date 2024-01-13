using Application.Dtos.Request.Teacher;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response.Teacher
{
    public class TeacherResponse
    {
        [MaxLength(150)] public string? Field { get; set; }

        [MaxLength(3000)] public string? Description { get; set; }
        public List<TeacherProfileRequest> Profile { get; set; }= new List<TeacherProfileRequest>();
        public List<TeacherContactRequest> Contact { get; set; } = new List<TeacherContactRequest>();
        public List<TeacherResourceRequest> Resource { get; set; } = new List<TeacherResourceRequest>();
    }
}
