using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TeacherRepository : BaseRepository<Teacher>,ITeacherRepository
    {
        public TeacherRepository(AppDbContext context, ILogger<BaseRepository<Teacher>> logger) : base(context, logger)
        {
        }

        public override async Task<Teacher?> GetByIdAsync(int id, bool disableTracking = false)
        {
            return await _context.Teachers.Where(x => x.Id == id)
                .Include(x => x.TeacherContactInformations)
                .Include(x => x.TeacherProfiles)
                .Include(x => x.TeacherResources).FirstOrDefaultAsync();
        }
    }
}