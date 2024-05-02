using Domain.Enums;

namespace Application.Dtos.Response;

public class ClassesResponse
{
    public ClassStatus ClassStatus { get; set; }
    public int CourseId { get; set; }
    public string? CourseImage { get; set; }
    public string? CourseName { get; set; }
    public int ClassId { get; set; }
    public string? ClassCode { get; set; }
    public string? OpenClass { get; set; }
    public string? DayOfWeekStart { get; set; }
    public string? CloseClass { get; set; }
    public string? DayOfWeekEnd { get; set; }
    public List<DayStatus>? Days { get; set; } = new List<DayStatus>();
    public string? StartSlot { get; set; }
    public string? EndSlot { get; set; }
    public string? TeacherName { get; set; }
    public string? RoomUrl { get; set; }
    public int Duration { get; set; }
    public int SlotDuration { get; set; }
    public double CourseProgress { get; set; } = 0;
}