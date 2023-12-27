using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IRoleService
    {
        Task<bool> AddRole(Role role);
        List<Role> GetRoles();
        Task<bool> UpdateRole(Role role);
    }
}
