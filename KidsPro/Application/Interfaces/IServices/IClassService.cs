using Application.Dtos.Request.Class;
using Application.Dtos.Response;

namespace Application.Interfaces.IServices;

public interface IClassService
{
     Task<ClassCreateResponse> CreateClassAsync(ClassCreateRequest dto);
     Task<ScheduleCreateResponse> CreateScheduleAsync(ScheduleCreateRequest dto);
     Task<List<TeacherScheduleResponse>> GetTeacherToClassAsync();

     Task<string> AddTeacherToClassAsync(int teacherId, int classId);
     Task<ClassResponse> GetClassByIdAsync(int classId);
     Task<ScheduleResponse> GetScheduleByClassIdAsync(int classId);
}