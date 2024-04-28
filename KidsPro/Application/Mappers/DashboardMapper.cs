using Application.Configurations;
using Application.Dtos.Response;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappers;

public static class DashboardMapper
{
    public static DashboardResponse ShowDashboardResponse(List<Course> courses, List<Order> orders,
        List<Account> accounts)
    {
        return new DashboardResponse
        {
            Courses = new List<Statistic>
            {
                CourseToStatistics(courses, CourseStatus.Draft),
                CourseToStatistics(courses, CourseStatus.Pending),
                CourseToStatistics(courses, CourseStatus.Denied),
                CourseToStatistics(courses, CourseStatus.Active),
                CourseToStatistics(courses, CourseStatus.Inactive),
                CourseToStatistics(courses, CourseStatus.Waiting),
            },
            Orders = new List<Statistic>
            {
                OrderToStatistics(orders, OrderStatus.Process),
                OrderToStatistics(orders, OrderStatus.Pending),
                OrderToStatistics(orders, OrderStatus.Success),
                OrderToStatistics(orders, OrderStatus.RequestRefund),
                OrderToStatistics(orders, OrderStatus.Refunded),
            },
            Account = new List<Statistic>
            {
                AccountToStatistics(accounts,Constant.StaffRole),
                AccountToStatistics(accounts,Constant.TeacherRole),
                AccountToStatistics(accounts,Constant.ParentRole),
                AccountToStatistics(accounts,Constant.StudentRole),
            },
        };
        
    }

    private static Statistic CourseToStatistics(List<Course> courses, CourseStatus status) => new Statistic
    {
        Total = courses.Count(x => x.Status == status),
        Status = status.ToString()
    };

    private static Statistic OrderToStatistics(List<Order> orders, OrderStatus status) => new Statistic
    {
        Total = orders.Count(x => x.Status == status),
        Status = status.ToString()
    };

    private static Statistic AccountToStatistics(List<Account> accounts, string role) => new Statistic
    {
        Total = accounts.Count(x => x.Role.Name == role),
        Status = role
    };
}