using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories
{
    public interface IRefeshTokenRepository:IBaseRepository<RefreshToken>
    {
        public bool CheckRefeshTokenExist(string? parameter,int type);
    }
}
