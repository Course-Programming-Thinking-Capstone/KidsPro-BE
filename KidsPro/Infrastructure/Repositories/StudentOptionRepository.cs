using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class StudentOptionRepository:BaseRepository<StudentOption>,IStudentOptionRepository
{
    public StudentOptionRepository(AppDbContext context, ILogger<BaseRepository<StudentOption>> logger) : base(context, logger)
    {
    }
    
    
}