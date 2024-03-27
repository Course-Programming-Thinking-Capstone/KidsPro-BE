using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Dtos.Request.Class;

public class ScheduleUpdateRequest
{
    [Required]
    public int ClassId { get; set; } 
    [Required]
    public List<DayStatus>? StudyDay  { get; set; }= new List<DayStatus>();
    [Required]
    public int SlotNumber { get; set; }
    [Required]
    public int SlotTime { get; set; }
    [Required]
    public string? RoomUrl { get; set; }
}