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

namespace Infrastructure.Repositories
{
    public class StudentProgressRepository : BaseRepository<StudentProgress>, IStudentProgressRepository
    {
        public StudentProgressRepository(AppDbContext context, ILogger<BaseRepository<StudentProgress>> logger) : base(context, logger)
        {
        }
    }
}
