using Application.Dtos.Request.Teacher;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface ITeacherContactService
    {
        public Task CreateOrUpdate(TeacherRequestType type, TeacherContactRequest dto);
    }
}