namespace Application.Dtos.Response.Course
{
    public class CourseOrderDto
    {
        public int CourseId { get; set; }
        public int? TeacherId { get; set; }
        public string? Picture { get; set; }
        public string? CourseName { get; set; }
        public string? TeacherName { get; set; }
        public decimal? Price { get; set; }
    }
}
