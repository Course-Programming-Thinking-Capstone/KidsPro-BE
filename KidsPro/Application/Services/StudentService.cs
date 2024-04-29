using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Student;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class StudentService : IStudentService
{
    private IUnitOfWork _unitOfWork;
    private IAccountService _accountService;
    private IClassService _classService;
    private IProgressService _progressService;

    public StudentService(IUnitOfWork unitOfWork, IAccountService accountService, IClassService classService,
        IProgressService progressService)
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
        _classService = classService;
        _progressService = progressService;
    }

    public async Task UpdateStudentAsync(StudentUpdateRequest dto)
    {
        var student = await _unitOfWork.StudentRepository.GetByIdAsync(dto.Id);
        if (student != null)
        {
            student.Account.FullName = StringUtils.FormatName(dto.FullName);
            student.Account.DateOfBirth = dto.BirthDay;
            student.Account.Gender = (Gender)(dto.Gender > 0 ? dto.Gender : 1);
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
        var progress = await _progressService.GetStudentCoursesProgressAsync(studentId);
        if (student != null)
            return StudentMapper.ShowStudentDetail(student,progress);
        throw new NotFoundException("studentId doesn't exist");
    }

    public async Task<List<StudentResponse>> GetStudentsAsync(int classId = 0)
    {
        var account = await _accountService.GetCurrentAccountInformationAsync();
        var students = await _unitOfWork.StudentRepository.GetStudents(account.Role, account.IdSubRole);

        if (classId == 0)
            return StudentMapper.ShowStudentList(students);

        var entityClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId)
                          ?? throw new NotFoundException($"ClassId: {classId} doesn't exist");

        var studentsWithoutCourse = GetStudentWithoutCourse(students, entityClass.CourseId);

        var studentsCanAddToClass = _classService.GetStudentsCanAddToClass(studentsWithoutCourse, entityClass);

        return StudentMapper.ShowStudentList(studentsCanAddToClass);
    }

    private List<Student> GetStudentWithoutCourse(List<Student> students, int courseId)
    {
        return students.Where(x => x.Classes
            .All(c => c.CourseId != courseId)).ToList();
    }
}