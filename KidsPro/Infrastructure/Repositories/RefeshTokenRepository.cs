using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RefeshTokenRepository : BaseRepository<RefeshToken>, IRefeshTokenRepository
    {
        public RefeshTokenRepository(AppDbContext context, ILogger<BaseRepository<RefeshToken>> logger) : base(context, logger)
        {
        }
    }
}
