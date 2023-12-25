using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CurriculumRepository: BaseRepository<Curriculum>, ICurriculumRepository
{
    public CurriculumRepository(AppDbContext context, ILogger<BaseRepository<Curriculum>> logger) : base(context, logger)
    {
    }
}