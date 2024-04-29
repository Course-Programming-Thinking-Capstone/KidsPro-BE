using Application.Dtos.Response;
using Domain.Enums;

namespace Application.Interfaces.IServices;

public interface IDashboardService
{
    Task<DashboardResponse> GetDashboardAsync();
}