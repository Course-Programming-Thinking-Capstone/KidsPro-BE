using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;
using Domain.Entities.Generic;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface ITeacherOverallRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        Task CreateOrUpdateAsync(TeacherRequestType type, Func<T> data);
        public Task<int> SaveChangeAsync();
    }
}