using Application.Dtos.Response.Certificate;
using Application.Dtos.Response.Course;

namespace Application.Dtos.Response.Account.Student
{
    public class StudentDetailResponse : StudentResponse
    {
        public string? UserName { get; set; }
      //  public string? Password { get; set; }
        public int CourseTotal { get; set; }
        public List<StudentCoursesDto>? StudentsCourse { get; set; } = new List<StudentCoursesDto>();
        public int CertificateTotal { get; set; }
        public List<CertificateDto>? StudentsCertificate { get; set; } = new List<CertificateDto>();

    }
}
