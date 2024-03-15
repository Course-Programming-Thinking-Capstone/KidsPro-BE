using Application.Configurations;
using Application.Dtos.Request.Student;
using Application.Dtos.Response.Student;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ParentsService: IParentsService
    {
        public IUnitOfWork _unitOfWork;

        public ParentsService(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        public async Task<StudentDto> AddStudent(StudentAddDto request)
        {
            var studentRole = await _unitOfWork.RoleRepository.GetByNameAsync(Constant.StudentRole)
                .ContinueWith(t => t.Result ?? throw new Exception("Role student name is incorrect."));

            var accountEntity = new Account()
            {
                PasswordHash = "000000",
                FullName = StringUtils.FormatName(request.FullName),
                Role = studentRole,
                DateOfBirth = request.Birthday,
                Gender = (Gender)request.Gender,
                CreatedDate = DateTime.UtcNow,
                Status = UserStatus.Active
            };

            var studentEntity = new Student()
            {
                ParentId = request.ParentId,
                Account = accountEntity
            };

            await _unitOfWork.StudentRepository.AddAsync(studentEntity);
            await _unitOfWork.SaveChangeAsync();

            var result = AccountMapper.AccountToStudentDto(accountEntity);
            return result;
        }

        public async Task<List<StudentDto>> GetStudents(int parentId)
        {
            var list = await _unitOfWork.StudentRepository.GetStudents(parentId);
            return AccountMapper.ParentToListStudentDto(list);
        }

        public async Task<StudentDetailDto> GetDetailStudent(int studentId)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            return AccountMapper.ShowStudentDetail(student);
        }

        public async Task UpdateStudent(StudentUpdateDto dto)
        {
            var _student= await _unitOfWork.StudentRepository.GetByIdAsync(dto.Id);
            if (_student != null)
            {
                _student.Account.FullName = StringUtils.FormatName(dto.FullName);
                _student.Account.DateOfBirth = dto.BirthDay;
                _student.Account.Gender = (Gender)dto.Gender;
                _student.Account.Email = dto.Email;
                _student.Account.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password);

                _unitOfWork.StudentRepository.Update(_student);
                await _unitOfWork.SaveChangeAsync();
            }
            else
                throw new NotFoundException($"Student Id {dto.Id} not found");
        }

    }
}
