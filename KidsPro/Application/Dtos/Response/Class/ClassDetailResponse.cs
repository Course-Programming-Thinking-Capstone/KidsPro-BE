using Application.Dtos.Response.StudentSchedule;
using Domain.Enums;

namespace Application.Dtos.Response;

public class ClassDetailResponse
{
    public int ClassId { get; set; }
    public string? ClassCode { get; set; }
    public string? CourseName { get; set; }
    public int TotalStudent { get; set; }
    public int? TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public string? OpenClass { get; set; }
    public string? CloseClass { get; set; }
    public int Duration { get; set; }
    public int SlotDuration { get; set; }
    public IEnumerable<DayStatus>? StudyDay  { get; set; }= new List<DayStatus>();
    public int? SlotNumber { get; set; }
    public TimeSpan? StartSlot { get; set; }
    public TimeSpan? EndSlot { get; set; }
    public int? TotalSlot { get; set; }
    public string? RoomUrl { get; set; }
    public List<StudentClassResponse>? Students  { get; set; }= new List<StudentClassResponse>();

}