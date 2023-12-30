using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CourseRepository:BaseRepository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context, ILogger<BaseRepository<Course>> logger) : base(context, logger)
    {
    }
}