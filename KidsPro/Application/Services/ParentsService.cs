using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Parent;
using Application.Dtos.Response.Account.Student;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

    public class ParentsService : IParentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authentication;

        public ParentsService(IUnitOfWork unitOfWork, IAuthenticationService authentication)
        {
            _unitOfWork = unitOfWork;
            _authentication = authentication;
        }

        private async Task<Parent?> GetInformationCurrentAsync()
        {
            _authentication.GetCurrentUserInformation(out var accountId, out var role);
            if (String.Compare(role, Constant.ParentRole, StringComparison.Ordinal) == 0)
                return await _unitOfWork.ParentRepository.GetByIdAsync(accountId);
            throw new BadRequestException("Account doesn't Parent Role");
        }

        public async Task<StudentResponseDto> AddStudentAsync(StudentAddRequestDto request)
        {
            var studentRole = await _unitOfWork.RoleRepository.GetByNameAsync(Constant.StudentRole)
                .ContinueWith(t => t.Result ?? throw new Exception("Role student name is incorrect."));

            var parent = await GetInformationCurrentAsync();

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
                ParentId = parent != null ? parent.Id : throw new NotFoundException("ParentId not found"),
                Account = accountEntity
            };

            await _unitOfWork.StudentRepository.AddAsync(studentEntity);
            await _unitOfWork.SaveChangeAsync();

            var result = AccountMapper.AccountToStudentDto(accountEntity);
            return result;
        }

        public async Task<List<StudentResponseDto>> GetStudentsAsync()
        {
            var parent = await GetInformationCurrentAsync();
            if (parent != null)
            {
                var list = await _unitOfWork.StudentRepository.GetStudents(parent.Id);
                return ParentMapper.ParentShowListStudent(list);
            }

            throw new UnauthorizedException("Parent doesn't exist");
        }

        public async Task<StudentDetailResponseDto> GetDetailStudentAsync(int studentId)
        {
            var student = await _unitOfWork.StudentRepository.GetStudentInformation(studentId);
            if (student != null)
                return ParentMapper.ParentShowStudentDetail(student);
            throw new NotFoundException("studentId doesn't exist");
        }

        public async Task UpdateStudentAsync(StudentUpdateRequestDto dto)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(dto.Id);
            if (student != null)
            {
                student.Account.FullName = StringUtils.FormatName(dto.FullName);
                student.Account.DateOfBirth = dto.BirthDay;
                student.Account.Gender = (Gender)dto.Gender;
                student.Account.Email = dto.Email;
                student.Account.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password);

                _unitOfWork.StudentRepository.Update(student);
                await _unitOfWork.SaveChangeAsync();
            }
            else
                throw new NotFoundException($"Student Id {dto.Id} not found");
        }

        public async Task<ParentOrderResponseDto> GetEmailZalo()
        {
            var parent = await GetInformationCurrentAsync();
            if (parent != null)
            {
                var result = _unitOfWork.ParentRepository.GetEmailZalo(parent.Id);
                if (result != null)
                    return ParentMapper.ParentShowEmailZalo(result);
            }

            throw new NotFoundException("Parent doesn't exist");
        }
    }
