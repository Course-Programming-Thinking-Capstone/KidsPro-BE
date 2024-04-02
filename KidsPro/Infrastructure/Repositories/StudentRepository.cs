using Application.Configurations;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
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

    public async Task<Student?> GameStudentLoginAsync(string account)
    {
        return await _dbSet.Include(s => s.Account)
            .ThenInclude(a => a.Role)
            .Include(s => s.GameUserProfile)
            .FirstOrDefaultAsync(x => x.UserName == account);
    }

    public async Task<List<Student>> GetStudents(string role,int parentId=0)
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
    
    public async Task<Student?> WebStudentLoginAsync(string account)
    {
        return await _dbSet.Include(x => x.Account).ThenInclude(x=> x.Role)
            .FirstOrDefaultAsync(x => x.UserName == account);
    }

    public async Task<List<Student>> SearchStudent(string input, SearchType type)
    {
        IQueryable<Student> query = _dbSet.AsNoTracking();
        switch (type)
        {
            case SearchType.ClassStudent:
                query = query.Include(x => x.Classes)
                    .ThenInclude(x => x.Schedules);
                break;
        }

        return await query.Include(x => x.Account)
            .Where(x => x.Account.FullName.ToLower().Trim().Contains(input.ToLower().Trim()))
            .ToListAsync();
    }

    public async Task<List<Student>> GetStudentsByIds(List<int> ids)
    {
        IQueryable<Student> query = _dbSet;

        return await query.Where(x=> ids.Contains(x.Id)) .Include(x => x.Account)
            .Include(x=> x.Parent).ThenInclude(x=> x.Account).ToListAsync();
    }

    public async Task<Student?> GetStudentProgress(int id)
    {
        var query = _dbSet.AsNoTracking();
        return await query.Include(x => x.StudentProgresses).ThenInclude(x=> x.Course)
            .Include(x => x.StudentProgresses).ThenInclude(x => x.Section)
            .ThenInclude(x => x.Lessons).ThenInclude(x => x.StudentLessons)
            .Include(x => x.StudentProgresses).ThenInclude(x => x.Section)
            .ThenInclude(x => x.Quizzes).ThenInclude(x => x.StudentQuizzes)
            .Include(x=>x.Account)
            .FirstOrDefaultAsync(x => x.Id == id);

    }
}