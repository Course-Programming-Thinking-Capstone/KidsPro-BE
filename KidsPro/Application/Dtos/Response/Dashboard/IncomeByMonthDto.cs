namespace Application.Dtos.Response;

public class IncomeByMonthDto
{
    public string? Month { get; set; }
    public List<StatisticDto> IncomeByWeek  { get; set; } = new List<StatisticDto>();
}