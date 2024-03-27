using Domain.Enums;

namespace Application.Dtos.Request.Class;

public class ScheduleCreateRequest
{
    public List<DayStatus> Days { get; set; } = new List<DayStatus>();
    public int ClassId { get; set; }
    public int Slot { get; set; }
    public int SlotTime { get; set; }
    public string? RoomUrl { get; set; }
}