using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CourseResourceRepository : BaseRepository<CourseResource>, ICourseResourceRepository
{
    public CourseResourceRepository(AppDbContext context, ILogger<BaseRepository<CourseResource>> logger) : base(
        context, logger)
    {
    }
}