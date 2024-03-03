using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class StaffRepository:BaseRepository<Staff>, IStaffRepository
{
    public StaffRepository(AppDbContext context, ILogger<BaseRepository<Staff>> logger) : base(context, logger)
    {
    }
}