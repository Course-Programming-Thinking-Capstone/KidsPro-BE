using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class ClassRepository:BaseRepository<Class>, IClassRepository
{
    public ClassRepository(AppDbContext context, ILogger<BaseRepository<Class>> logger) : base(context, logger)
    {
    }
}