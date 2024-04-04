using Domain.Enums;

namespace Application.Dtos.Response;

public class ClassesResponse
{
    public int ClassId { get; set; }
    public string? ClassCode { get; set; }
    public List<DayStatus>? Days { get; set; } = new List<DayStatus>();
    public string? Start { get; set; }
    public string? End { get; set; }
}