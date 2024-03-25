using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class ScheduleReposisoty:BaseRepository<ClassSchedule>,IScheduleReposisoty
{
    public ScheduleReposisoty(AppDbContext context, ILogger<BaseRepository<ClassSchedule>> logger) : base(context, logger)
    {
    }
}