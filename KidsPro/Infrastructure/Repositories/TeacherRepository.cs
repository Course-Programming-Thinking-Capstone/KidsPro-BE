using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class TeacherRepository:BaseRepository<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext context, ILogger<BaseRepository<Teacher>> logger) : base(context, logger)
    {
    }
}