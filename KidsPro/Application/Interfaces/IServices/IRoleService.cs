using Domain.Entities;

namespace Application.Interfaces.IServices
{
    public interface IRoleService
    {
        Task<bool> AddRole(Role role);
        List<Role> GetRoles();
        Task<bool> UpdateRole(Role role);
    }
}
