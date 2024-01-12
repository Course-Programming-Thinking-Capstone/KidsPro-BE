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
    public class TeacherContactService : ITeacherContactService
    {
        ITeacherRepository<TeacherContactInformation> _teacher;
        IMapper _map;

        public TeacherContactService(ITeacherRepository<TeacherContactInformation> teacher, IMapper map)
        {
            _teacher = teacher;
            _map = map;
        }

        public async Task CreateOrUpdate(TeacherRequestType type, TeacherContactRequest dto)
        {
            var _contact = await _teacher.GetByIdAsync(dto.Id);
            switch (type)
            {
                case TeacherRequestType.Create:
                    if (_contact == null)
                        _contact = new TeacherContactInformation();
                    else
                        throw new BadRequestException("Teacher Contact Information is existed, create failed");
                    break;
                case TeacherRequestType.Update:
                    if (_contact == null)
                        throw new BadRequestException("Teacher Contact Information is not existed, update failed");
                    break;
            }
            await _teacher.CreateOrUpdateAsync(type, () =>
            {
                _contact = _map.Map<TeacherContactInformation>(dto);
                return _contact;
            });
        }
    }
}