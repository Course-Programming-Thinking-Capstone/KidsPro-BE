using Application.Dtos.Request.Teacher;
using Application.ErrorHandlers;
using Domain.Entities;
using Domain.Entities.Generic;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface ITeacherService
    {
        Task CreateTeacher(int id);
        Task<Teacher?> GetTeacherById(int id);
        Task<bool> UpdateTeacher(TeacherRequest request);
    }
}