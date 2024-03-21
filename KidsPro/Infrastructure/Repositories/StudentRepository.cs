using Application.Configurations;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class StudentRepository:BaseRepository<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext context, ILogger<BaseRepository<Student>> logger) : base(context, logger)
    {
    }

    public async Task<Student?> GameStudentLoginAsync(string email)
    {
        return await _dbSet.Include(s => s.Account)
            .ThenInclude(a => a.Role)
            .Include(s => s.GameUserProfile)
            .FirstOrDefaultAsync(s => s.Account.Email == email);
    }

    public async Task<List<Student>> GetStudents(int parentId,string role)
    {
        var query = _dbSet.AsNoTracking();
        if (role == Constant.ParentRole)
        {
            query =query.Where(x => x.ParentId == parentId);
        }
        return await query.Include(x => x.Account)
            .Include(x=> x.Parent).ThenInclude(x=> x.Account).ToListAsync();
    }

    public override async Task<Student?> GetByIdAsync(int id, bool disableTracking = false)
    {
        return await _dbSet.Where(x => x.Id == id)
            .Include(x => x.Account).ThenInclude(x => x.Role)
            .Include(x=> x.Parent).ThenInclude(x=> x.Account)
            .FirstOrDefaultAsync();
    }

    public async Task<Student?> GetStudentInformation(int id)
    {
        return await _dbSet.Where(x => x.Id == id)
            .Include(x => x.Account).ThenInclude(x => x.Role)
            .Include(x => x.Certificates)
            .Include(x => x.StudentProgresses)!.ThenInclude(x => x.Course)
            .Include(x=> x.Parent).ThenInclude(x=> x.Account)
            .FirstOrDefaultAsync();
    }
}