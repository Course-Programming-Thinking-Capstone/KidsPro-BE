using Application.ErrorHandlers;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Entities.Generic;
using Domain.Enums;
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
    public class TeacherRepository<T> : BaseRepository<T>, ITeacherRepository<T> where T : BaseEntity
    {
        public TeacherRepository(AppDbContext context, ILogger<BaseRepository<T>> logger) : base(context, logger)
        {
        }
        public async Task CreateOrUpdateAsync(TeacherRequestType type, Func<T> data)
        {
            T entity = data();
            if (entity == null)
                throw new ConflictException("Etity data is empty");
            switch (type)
            {
                case TeacherRequestType.Create:
                    await AddAsync(entity);
                    break;
                case TeacherRequestType.Update:
                    Update(entity);
                    break;
            }
            var result = await SaveChangeAsync();
            if (result <= 0)
                throw new NotImplementException("Save Data Failed");
        }
        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}