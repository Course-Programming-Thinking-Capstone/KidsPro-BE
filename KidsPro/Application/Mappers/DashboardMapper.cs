using Application.Configurations;
using Application.Dtos.Response;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappers;

public static class DashboardMapper
{
    public static DashboardResponse ShowDashboardResponse(List<Course> courses, List<Order> orders,
        List<Account> accounts, List<Transaction> transactions)
    {
        return new DashboardResponse
        {
            Courses = new List<StatisticDto>
            {
                CourseToStatistics(courses, CourseStatus.Draft),
                CourseToStatistics(courses, CourseStatus.Pending),
                CourseToStatistics(courses, CourseStatus.Denied),
                CourseToStatistics(courses, CourseStatus.Active),
                CourseToStatistics(courses, CourseStatus.Inactive),
                CourseToStatistics(courses, CourseStatus.Waiting),
                new StatisticDto
                {
                    Total = courses.Count,
                    Status = "All Courses"
                }
            },
            Orders = new List<StatisticDto>
            {
                OrderToStatistics(orders, OrderStatus.Process),
                OrderToStatistics(orders, OrderStatus.Pending),
                OrderToStatistics(orders, OrderStatus.Success),
                OrderToStatistics(orders, OrderStatus.RequestRefund),
                OrderToStatistics(orders, OrderStatus.Refunded),
                new StatisticDto
                {
                    Total = orders.Count,
                    Status = "All Orders"
                }
            },
            Account = new List<StatisticDto>
            {
                AccountToStatistics(accounts, Constant.StaffRole),
                AccountToStatistics(accounts, Constant.TeacherRole),
                AccountToStatistics(accounts, Constant.ParentRole),
                AccountToStatistics(accounts, Constant.StudentRole),
                new StatisticDto
                {
                    Total = accounts.Count,
                    Status = "All Accounts"
                }
            },
            NewUserThisMonth = new List<StatisticDto>
            {
                NewUserThisMonth(accounts, Constant.ParentRole),
                NewUserThisMonth(accounts, Constant.StudentRole)
            },
            MonthlyEarning = new List<StatisticDto>
            {
                MonthlyEarning(transactions, "ThisMonth"),
                MonthlyEarning(transactions, "LastMonth"),
                EarningPercent(transactions, "")
            },
            IncomeByMonth = new List<StatisticDto>
            {
                MonthlyEarning(transactions, MonthType.January),
                MonthlyEarning(transactions, MonthType.February),
                MonthlyEarning(transactions, MonthType.March),
                MonthlyEarning(transactions, MonthType.April),
                MonthlyEarning(transactions, MonthType.May),
                MonthlyEarning(transactions, MonthType.June),
                MonthlyEarning(transactions, MonthType.July),
                MonthlyEarning(transactions, MonthType.August),
                MonthlyEarning(transactions, MonthType.September),
                MonthlyEarning(transactions, MonthType.October),
                MonthlyEarning(transactions, MonthType.November),
                MonthlyEarning(transactions, MonthType.December),
            },
            //IncomeByMonth = IncomeByMonth(transactions, month)
        };
    }

    private static StatisticDto CourseToStatistics(List<Course> courses, CourseStatus status) => new StatisticDto
    {
        Total = courses.Count(x => x.Status == status),
        Status = status.ToString()
    };

    private static StatisticDto OrderToStatistics(List<Order> orders, OrderStatus status) => new StatisticDto
    {
        Total = orders.Count(x => x.Status == status),
        Status = status.ToString()
    };

    private static StatisticDto AccountToStatistics(List<Account> accounts, string role) => new StatisticDto
    {
        Total = accounts.Count(x => x.Role.Name == role),
        Status = role
    };

    private static StatisticDto NewUserThisMonth(List<Account> accounts, string role) => new StatisticDto
    {
        Total = accounts.Count(x => x.Role.Name == role && x.CreatedDate.Month == DateTime.UtcNow.Month),
        Status = role
    };

    private static StatisticDto MonthlyEarning(List<Transaction> transactions, string month)
    {
        var result = new StatisticDto();

        var currentMonth = DateTime.UtcNow.Month;
        var lastMonth = currentMonth == 1 ? 12 : currentMonth - 1;

        switch (month)
        {
            case "ThisMonth":
                result.Total = transactions
                    .Where(x => x.CreatedDate.Month == currentMonth)
                    .Sum(x => x.Amount);
                break;
            case "LastMonth":
                result.Total = transactions
                    .Where(x => x.CreatedDate.Month == lastMonth)
                    .Sum(x => x.Amount);
                break;
        }

        result.Status = month;
        return result;
    }

    private static StatisticDto MonthlyEarning(List<Transaction> transactions, MonthType month)
    {
        var result = new StatisticDto();

        result.Total = transactions
            .Where(x => x.CreatedDate.Month == (int)month)
            .Sum(x => x.Amount);

        result.Status = month.ToString();
        return result;
    }

    private static StatisticDto EarningPercent(List<Transaction> transactions, string month)
    {
        var currentMonth = DateTime.UtcNow.Month;
        var lastMonth = currentMonth == 1 ? 12 : currentMonth - 1;

        var totalThisMonth = Convert.ToInt32(transactions
            .Where(x => x.CreatedDate.Month == currentMonth)
            .Sum(x => x.Amount));
        var totalLastMonth = Convert.ToInt32(transactions
            .Where(x => x.CreatedDate.Month == lastMonth)
            .Sum(x => x.Amount));

        var percent = totalLastMonth != 0 ? ((totalThisMonth - totalLastMonth) / (double)totalLastMonth) * 100 : 100;

        return new StatisticDto
        {
            Total = Math.Round(Convert.ToDecimal(percent), 2),
            Status = percent > 0 ? "Increase" : percent < 0 ? "Decrease" : "No Change"
        };
    }

    private static IncomeByMonthDto IncomeByMonth(List<Transaction> transactions, MonthType month)
    {
        var incomeByWeek = new int[4];
        var incomes = new List<StatisticDto>();

        for (int i = 0; i < 4; i++)
        {
            int startDay = i * 7 + 1;
            int endDay = startDay + 8;

            incomeByWeek[i] = Convert.ToInt32(transactions
                .Where(x => x.CreatedDate.Day >= startDay && x.CreatedDate.Day <= endDay &&
                            x.CreatedDate.Month == (int)month)
                .Sum(x => x.Amount));

            var week = new StatisticDto
            {
                Total = incomeByWeek[i],
                Status = "Week " + (i + 1) + " (Day " + startDay + " - " + endDay + ")"
            };
            incomes.Add(week);
        }

        return new IncomeByMonthDto
        {
            Month = month.ToString(),
            IncomeByWeek = incomes
        };
    }
}