using System.Reflection.Metadata;
using Application.Dtos.Response;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Enums;
using Constant = Application.Configurations.Constant;

namespace Application.Services;

public class DashboardService:IDashboardService
{
    private IUnitOfWork _unitOfWork;
    private IAccountService _accountService;

    public DashboardService(IUnitOfWork unitOfWork, IAccountService accountService)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
    }

    public async Task<DashboardResponse> GetDashboardAsync()
    {
        // var account = await _accountService.GetCurrentAccountInformationAsync();
        // if (account.Role != Constant.AdminRole)
        //     throw new UnauthorizedException("Please login by account admin");

        var orders = await _unitOfWork.OrderRepository.GetAllAsync();
        var courses = await _unitOfWork.CourseRepository.GetAllAsync();
        var accounts = await _unitOfWork.AccountRepository.GetAllAsync();
        var transactions= await _unitOfWork.TransactionRepository.GetAllAsync();
        return DashboardMapper.ShowDashboardResponse(courses, orders, accounts);
    }
}