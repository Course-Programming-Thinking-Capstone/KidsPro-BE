namespace Application.Dtos.Response;

public class DashboardResponse
{
    public List<StatisticDto> Orders { get; set; } = new List<StatisticDto>();
    public List<StatisticDto> Courses { get; set; } = new List<StatisticDto>();
    public List<StatisticDto> Account { get; set; } = new List<StatisticDto>();
    public List<StatisticDto> NewUserThisMonth { get; set; } = new List<StatisticDto>();
    public List<StatisticDto> MonthlyEarning { get; set; } = new List<StatisticDto>();
    public IncomeByMonthDto? IncomeByMonth { get; set; }
}