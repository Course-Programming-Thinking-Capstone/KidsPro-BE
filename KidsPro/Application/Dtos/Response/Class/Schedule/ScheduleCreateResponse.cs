using Domain.Enums;

namespace Application.Dtos.Response;

public class ScheduleCreateResponse
{
    public List<DayStatus> Days { get; set; } = new List<DayStatus>();
    public int ClassId { get; set; }
    public int Slot { get; set; }
    public TimeSpan Open { get; set; }
    public TimeSpan Close { get; set; }
    public string? Link { get; set; }
}