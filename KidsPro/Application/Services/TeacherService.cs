using Application.Dtos.Request.Teacher;
using Application.ErrorHandlers;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Generic;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TeacherService : ITeacherService
    {
        ITeacherOverallRepository<Teacher> _teacher;

        public TeacherService(ITeacherOverallRepository<Teacher> unit)
        {
            _teacher = unit;
        }

        public async Task CreateTeacher(int id)
        {
            if (id > 0)
            {
                var teacher = new Teacher();
                teacher.UserId = id;
                await _teacher.AddAsync(teacher);
                await _teacher.SaveChangeAsync();
            }
            else
            {
                throw new NotImplementException("User id must be > 0");
            }
        }

    }
}
