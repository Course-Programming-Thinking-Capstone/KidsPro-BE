using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class PositionTypeRepository:BaseRepository<PositionType>, IPositionTypeRepository
{
    public PositionTypeRepository(AppDbContext context, ILogger<BaseRepository<PositionType>> logger) : base(context, logger)
    {
    }
}