using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StudentProgressRepository : BaseRepository<StudentProgress>, IStudentProgressRepository
    {
        public StudentProgressRepository(AppDbContext context, ILogger<BaseRepository<StudentProgress>> logger) : base(
            context, logger)
        {
        }

        public async Task<List<StudentProgress>> GetSectionProgress(int studentId, int courseId = 0)
        {
            var query = _dbSet.AsNoTracking();
            if (courseId > 0)
                query = query.Where(x => x.CourseId == courseId);
            return await _dbSet.Include(x => x.Course).ThenInclude(x => x.ModifiedBy)
                .Include(x => x.Section)
                .ThenInclude(x => x.Lessons).ThenInclude(x => x.StudentLessons)
                .Include(x => x.Section)
                .ThenInclude(x => x.Quizzes).ThenInclude(x => x.StudentQuizzes)
                .Include(x => x.Student).ThenInclude(x => x.Account)
                .Where(x => x.StudentId == studentId).ToListAsync();
        }

        public async Task<bool> CheckStudentSectionExistAsync(int studentId, int sectionId)
        {
            var entity= await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.SectionId == sectionId);
            return entity == null;
        }
    }
}