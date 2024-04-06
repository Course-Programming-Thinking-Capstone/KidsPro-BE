using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.Dtos.Request.Class;

public class ScheduleCreateRequest
{
    public List<DayStatus> Days { get; set; } = new List<DayStatus>();
    public int ClassId { get; set; }
    public int Slot { get; set; }
    public int SlotTime { get; set; }
    [JsonIgnore] public string? RoomUrl { get; set; } = string.Empty;
}