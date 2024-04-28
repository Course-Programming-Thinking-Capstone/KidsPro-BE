using System.Reflection.Metadata;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Enums;
using Constant = Application.Configurations.Constant;

namespace Application.Services;

public class DashboardService
{
    private IUnitOfWork _unitOfWork;
    private IAccountService _accountService;

    public DashboardService(IUnitOfWork unitOfWork, IAccountService accountService)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
    }

    public async Task GetDashboardAsync()
    {
        var account = await _accountService.GetCurrentAccountInformationAsync();
        if (account.Role != Constant.AdminRole)
            throw new UnauthorizedException("Please login by account admin");

        var orders = await _unitOfWork.OrderRepository.MobileGetListOrderAsync(OrderStatus.AllStatus,0,Constant.AdminRole);
    }
}