using Domain.Enums;

namespace Application.Dtos.Response;

public class StatisticDto
{
    public decimal Total { get; set; }
    public string? Status { get; set; }
}