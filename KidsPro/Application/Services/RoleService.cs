using Application.Interfaces.IServices;
using Domain.Entities;

namespace Application.Services
{
    public class RoleService : IRoleService
    {
        IUnitOfWork _unit;

        public RoleService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<bool> AddRole(Role role)
        {
            if(role != null)
            {
                await _unit.RoleRepository.AddAsync(role);
                var result=await _unit.SaveChangeAsync();
                if (result > 0) return true;
            }
            return false;
        }

        public List<Role> GetRoles()
        {
            return  _unit.RoleRepository.GetAll().ToList();
        }

        public async Task<bool> UpdateRole(Role role)
        {
           var roleExist= await _unit.RoleRepository.GetByIdAsync(role.Id);
            if (roleExist != null)
            {
                roleExist.Name= role.Name;
                _unit.RoleRepository.Update(role);
                var result = await _unit.SaveChangeAsync();
                if (result > 0) return true;
            }
            return false;
        }
    }
}
