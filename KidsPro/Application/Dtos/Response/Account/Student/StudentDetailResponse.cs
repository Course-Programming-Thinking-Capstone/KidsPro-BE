using Application.Dtos.Response.Certificate;
using Application.Dtos.Response.Course;

namespace Application.Dtos.Response.Account.Student
{
    public class StudentDetailResponse : StudentResponse
    {
        public string? UserName { get; set; }
      //  public string? Password { get; set; }
        public int CourseTotal { get; set; }
        public List<TitleDto>? StudentsCourse { get; set; } = new List<TitleDto>();
        public int CertificateTotal { get; set; }
        public List<CertificateResponseDto>? StudentsCertificate { get; set; } = new List<CertificateResponseDto>();
    }
}
