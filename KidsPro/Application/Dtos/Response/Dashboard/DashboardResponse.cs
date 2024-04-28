namespace Application.Dtos.Response;

public class DashboardResponse
{
    public List<Statistic> Orders { get; set; } = new  List<Statistic>();
    public List<Statistic> Courses { get; set; } = new List<Statistic>();
    public List<Statistic> Account { get; set; } = new List<Statistic>();
}