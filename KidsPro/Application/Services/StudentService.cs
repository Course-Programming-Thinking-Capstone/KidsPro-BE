using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Student;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Enums;

namespace Application.Services;

public class StudentService:IStudentService
{
    private IUnitOfWork _unitOfWork;
    private IAccountService _accountService;

    public StudentService(IUnitOfWork unitOfWork, IAccountService accountService)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
    }

    public async Task UpdateStudentAsync(StudentUpdateRequest dto)
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
    
    public async Task<StudentDetailResponse> GetDetailStudentAsync(int studentId)
    {
        var student = await _unitOfWork.StudentRepository.GetStudentInformation(studentId);
        if (student != null)
            return StudentMapper.ShowStudentDetail(student);
        throw new NotFoundException("studentId doesn't exist");
    }
    
    public async Task<List<StudentResponse>> GetStudentsAsync()
    {
        var account = await _accountService.GetCurrentAccountInformationAsync();
        var list = await _unitOfWork.StudentRepository.GetStudents(account.IdSubRole,account.Role);
        return StudentMapper.ShowStudentList(list);
    }
    
    
}