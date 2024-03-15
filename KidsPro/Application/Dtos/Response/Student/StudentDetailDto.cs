using Application.Dtos.Response.Certificate;
using Application.Dtos.Response.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Response.Student
{
    public class StudentDetailDto : StudentDto
    {
        public int CourseTotal { get; set; }
        public List<TitleDto> StudentsCourse { get; set; } = new List<TitleDto>();
        public int CertificateTotal { get; set; }
        public List<CertificateDto> StudentsCertificate { get; set; } = new List<CertificateDto>();
    }
}
