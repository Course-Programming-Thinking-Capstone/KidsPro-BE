using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class RefeshTokenRepository : BaseRepository<RefreshToken>, IRefeshTokenRepository
    {
        public RefeshTokenRepository(AppDbContext context, ILogger<BaseRepository<RefreshToken>> logger) : base(context, logger)
        {
        }

        public bool CheckRefeshTokenExist(string? parameter, int type)
        {
            if(parameter == null) return false;
            RefreshToken? result= null;
            switch (type)
            {
                case 1://Check by UserId
                    result = _context.Tokens.Where(x => x.UserId.Equals(parameter)).FirstOrDefault();
                    break;
                case 2://Check by Token
                    result = _context.Tokens.Where(x => x.Token.Equals(parameter)).FirstOrDefault();
                    break;
            }
            if(result !=null)
                return true;
            return false;
        }
    }
}
