using Application.Dtos.Request.Teacher;
using Application.ErrorHandlers;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TeacherProfileService:ITeacherProfileService
    {
        ITeacherRepository<TeacherProfile> _teacher;
        IMapper _map;

        public TeacherProfileService(ITeacherRepository<TeacherProfile> teacher, IMapper map)
        {
            _teacher = teacher;
            _map = map;
        }

        public async Task CreateOrUpdate(TeacherRequestType type, TeacherProfileRequest dto)
        {
            var _contact = await _teacher.GetByIdAsync(dto.Id);
            switch (type)
            {
                case TeacherRequestType.Create:
                    if (_contact == null)
                        _contact = new TeacherProfile();
                    else
                        throw new BadRequestException("Teacher profile is existed, create failed");
                    break;
                case TeacherRequestType.Update:
                    if (_contact == null)
                        throw new BadRequestException("Teacher profile is not existed, update failed");
                    break;
            }
            await _teacher.CreateOrUpdateAsync(type, () =>
            {
                _contact = _map.Map<TeacherProfile>(dto);
                return _contact;
            });
        }
    }
}
