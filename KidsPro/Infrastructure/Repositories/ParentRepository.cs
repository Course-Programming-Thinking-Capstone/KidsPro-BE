using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class ParentRepository:BaseRepository<Parent>, IParentRepository
{
    public ParentRepository(AppDbContext context, ILogger<BaseRepository<Parent>> logger) : base(context, logger)
    {
    }
}