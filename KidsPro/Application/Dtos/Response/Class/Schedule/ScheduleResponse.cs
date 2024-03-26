using Domain.Enums;

namespace Application.Dtos.Response;

public class ScheduleResponse
{
    public int? SlotTime { get; set; }
    public IEnumerable<DayStatus>? StudyDay  { get; set; }= new List<DayStatus>();
    public int? SlotNumber { get; set; }
    public TimeSpan? StartSlot { get; set; }
    public TimeSpan? EndSlot { get; set; }
    public string? RoomUrl { get; set; }
}