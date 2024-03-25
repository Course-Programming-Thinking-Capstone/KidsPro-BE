using Domain.Enums;

namespace Application.Dtos.Request.Class;

public class ScheduleCreateRequest
{
    public List<DayStatus> Days { get; set; } 
    public int ClassId { get; set; }
    public int Slot { get; set; }
    public string? Link { get; set; }
    public int SlotTime { get; set; }
}