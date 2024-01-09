using Application.Configurations;
using Application.ErrorHandlers;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context, ILogger<BaseRepository<Role>> logger) : base(context, logger)
    {
    }
    public async Task<Role> GetRoleAsync(int number)
    {
        var _role= new Role();
        switch (number)
        {
            //case 1:
            //    _role = await GetAsync(filter: x => x.Name == Constant.AdminRole, orderBy: null)
            //        .ContinueWith(t => t.Result.FirstOrDefault() ?? throw new NotFoundException("Staff role not found on database."));
            //    break;
            case 2:
                _role =await GetAsync(filter: x => x.Name == Constant.StaffRole, orderBy: null)
                    .ContinueWith(t => t.Result.FirstOrDefault() ?? throw new NotFoundException("Staff role not found on database."));
                break;
            case 3:
                _role = await GetAsync(filter: x => x.Name == Constant.TeacherRole, orderBy: null)
                   .ContinueWith(t => t.Result.FirstOrDefault() ?? throw new NotFoundException("Teacher role not found on database."));
                break;
            case 4:
                _role = await GetAsync(filter: x => x.Name == Constant.ParentRole, orderBy: null)
                   .ContinueWith(t => t.Result.FirstOrDefault() ?? throw new NotFoundException("Parent role not found on database."));
                break;
        }
        return _role;
    }
}