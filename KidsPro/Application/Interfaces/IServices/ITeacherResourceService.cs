using Application.Dtos.Request.Teacher;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface ITeacherResourceService
    {
        public Task CreateOrUpdate(TeacherRequestType type, TeacherResourceRequest dto);
    }
}
