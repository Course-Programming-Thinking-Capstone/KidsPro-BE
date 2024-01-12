﻿using Application.ErrorHandlers;
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
        public Task CreateTeacher(int id);
    }
}