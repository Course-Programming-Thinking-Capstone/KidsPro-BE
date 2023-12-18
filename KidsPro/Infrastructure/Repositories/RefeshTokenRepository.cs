using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
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
        public override async Task<RefeshToken?> GetByIdAsync(object id)
        {
            return await _context.Tokens.Where(x=> x.UserId.Equals(id)).FirstOrDefaultAsync();
        }
    }
}
