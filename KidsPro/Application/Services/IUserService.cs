using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserService
    {
         Task<(string,string?)> LoginAsync(string phonenumber, string password);
         Task<User> GetUserById(Guid id);

    }
}
