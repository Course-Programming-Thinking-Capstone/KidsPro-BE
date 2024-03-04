using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class ItemOwnedRepository:BaseRepository<ItemOwned>, IItemOwnedRepository
{
    public ItemOwnedRepository(AppDbContext context, ILogger<BaseRepository<ItemOwned>> logger) : base(context, logger)
    {
    }
}