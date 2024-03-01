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
}