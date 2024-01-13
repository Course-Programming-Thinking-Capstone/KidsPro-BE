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
        IUnitOfWork _unit;

        public TeacherService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task CreateTeacher(int id)
        {
            if (id > 0)
            {
                var teacher = new Teacher();
                teacher.UserId = id;
                await _unit.TeacherRepository.AddAsync(teacher);
                await _unit.SaveChangeAsync();
            }
            else
                throw new NotImplementException("User id must be > 0");
        }

        public async Task<Teacher?> GetTeacherById(int id)
        {
            var result = await _unit.TeacherRepository.GetByIdAsync(id);
            if (result == null) throw new NotFoundException("Id not exist in database");
            return result;
        }

        public async Task<bool> UpdateTeacher(TeacherRequest request)
        {
            var teacher = await _unit.TeacherRepository.GetByIdAsync(request.Id);
            if(teacher!=null)
            {
                teacher.Description=request.Description;
                teacher.Field=request.Field;
                _unit.TeacherRepository.Update(teacher);
                var result = await _unit.SaveChangeAsync();
                if (result > 0)
                    return true;
                throw new NotImplementException("Save update failed!");
            }
            throw new NotFoundException("Id not exist in database");
        }

    }
}