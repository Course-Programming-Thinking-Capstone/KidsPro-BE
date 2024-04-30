using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Response;

public class ClassCreateResponse
{
    public int ClassId { get; set; }
    public string? ClassCode { get; set; }
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
    public string? OpenDay { get; set; }
    public string? DayOfWeekStart { get; set; }
    public string? CloseDay { get; set; }
    public string? DayOfWeekEnd { get; set; }
    public int ClassDuration { get; set; }
    public int? SlotDuration { get; set; }
    public int? SlotPerWeek { get; set; }
    public int? TotalSlot { get; set; }
}