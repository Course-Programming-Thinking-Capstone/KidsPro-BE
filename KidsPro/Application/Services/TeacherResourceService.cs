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
    public class TeacherResourceService : ITeacherResourceService
    {
        ITeacherOverallRepository<TeacherResource> _teacher;
        IMapper _map;

        public TeacherResourceService(ITeacherOverallRepository<TeacherResource> teacher, IMapper map)
        {
            _teacher = teacher;
            _map = map;
        }

        public async Task CreateOrUpdate(TeacherRequestType type, TeacherResourceRequest dto)
        {
            var _contact = await _teacher.GetByIdAsync(dto.Id);
            switch (type)
            {
                case TeacherRequestType.Create:
                    if (_contact == null)
                    {
                        _contact = new TeacherResource();
                        //Add Entity
                        await _teacher.CreateOrUpdateAsync(type, () =>
                        {
                            _contact = _map.Map<TeacherResource>(dto);
                            return _contact;
                        });
                    }
                    else
                        throw new BadRequestException("Teacher profile is existed, create failed");
                    break;
                case TeacherRequestType.Update:
                    if (_contact == null)
                        throw new NotFoundException("Teacher profile is not existed, update failed");
                    else
                    {
                        //Update Entity
                        await _teacher.CreateOrUpdateAsync(type, () =>
                        {
                            _contact.TeacherId = dto.TeacherId;
                            _contact.ResourceUrl = dto.ResourceUrl;
                            _contact.Type = dto.Type;
                            return _contact;
                        });
                    }
                    break;
            }
        }


    }
}