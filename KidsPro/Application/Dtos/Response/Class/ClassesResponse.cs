using Domain.Enums;

namespace Application.Dtos.Response;

public class ClassesResponse
{
    public int ClassId { get; set; }
    public string? ClassCode { get; set; }
    public string? DayStart { get; set; }
    public string? DayOfWeekStart { get; set; }
    public string? DayEnd { get; set; }
    public string? DayOfWeekEnd { get; set; }
    public List<DayStatus>? Days { get; set; } = new List<DayStatus>();
    public string? SlotStart { get; set; }
    public string? SlotEnd { get; set; }
}