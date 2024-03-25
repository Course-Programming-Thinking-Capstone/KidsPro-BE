using Domain.Enums;

namespace Application.Dtos.Request.Class;

public class ScheduleCreateRequest
{
    public List<int> Days { get; set; } = new List<int>();
    public int ClassId { get; set; }
    public int Slot { get; set; }
    public int SlotTime { get; set; }
    public string? Link { get; set; }
}