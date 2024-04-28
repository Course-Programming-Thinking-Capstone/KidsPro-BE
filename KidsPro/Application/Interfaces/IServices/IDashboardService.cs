using Application.Dtos.Response;

namespace Application.Interfaces.IServices;

public interface IDashboardService
{
    Task<DashboardResponse> GetDashboardAsync();
}