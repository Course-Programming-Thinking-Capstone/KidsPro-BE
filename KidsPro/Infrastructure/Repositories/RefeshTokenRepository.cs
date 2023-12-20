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

        public bool CheckRefeshTokenExist(string? parameter, int type)
        {
            if(parameter == null) return false;
            RefeshToken? result= null;
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
