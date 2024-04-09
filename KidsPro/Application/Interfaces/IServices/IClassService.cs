using Application.Dtos.Request.Class;
using Application.Dtos.Response;
using Application.Dtos.Response.Account.Student;
using Application.Dtos.Response.Paging;
using Application.Dtos.Response.StudentSchedule;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.IServices;

public interface IClassService
{
     Task<ClassCreateResponse> CreateClassAsync(ClassCreateRequest dto);
     Task<ScheduleCreateResponse> CreateScheduleAsync(ScheduleCreateRequest dto);
     Task<List<TeacherScheduleResponse>> GetTeacherToClassAsync();

     Task<string> AddTeacherToClassAsync(int teacherId, int classId);
     Task<ClassDetailResponse> GetClassByIdAsync(int classId);
     Task<ScheduleResponse> GetScheduleByClassIdAsync(int classId);
     Task UpdateScheduleAsync(ScheduleUpdateRequest dto);
     Task<List<StudentClassResponse>> SearchStudentScheduleAsync(string input, int classId);
     Task<List<StudentClassResponse>> UpdateStudentsToClassAsync(StudentsAddRequest dto);
     Task<PagingClassesResponse> GetClassesAsync(int? page, int? size);
     Task<List<ClassesResponse>> GetClassByRoleAsync();
     List<Student> GetStudentsCanAddToClass(List<Student> students, Class entityClass);
}