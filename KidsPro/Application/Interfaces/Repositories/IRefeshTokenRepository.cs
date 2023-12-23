using Application.Interfaces.Repositories.Generic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IRefeshTokenRepository:IBaseRepository<RefreshToken>
    {
        public bool CheckRefeshTokenExist(string? parameter,int type);
    }
}
