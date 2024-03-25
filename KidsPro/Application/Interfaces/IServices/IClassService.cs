using Application.Dtos.Request.Class;
using Application.Dtos.Response;

namespace Application.Interfaces.IServices;

public interface IClassService
{
     Task<ClassCreateResponse> CreateClassAsync(ClassCreateRequest dto);
     Task<ScheduleCreateResponse> CreateScheduleAsync(ScheduleCreateRequest dto);
     Task<List<TeacherScheduleResponse>> GetTeacherToClassAsync();
}