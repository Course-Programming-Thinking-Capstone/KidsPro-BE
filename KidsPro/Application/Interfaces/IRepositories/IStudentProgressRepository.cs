using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IStudentProgressRepository:IBaseRepository<StudentProgress>
    {
        Task<List<StudentProgress>> GetSectionProgress(int studentId, int courseId=0);
        Task<bool> CheckStudentSectionExistAsync(int studentId, int sectionId);

        Task<StudentProgress?> GetStudentProgressAsync(int studentId, int courseId);
        Task<List<StudentProgress?>> CheckStudentProgressAsync(int studentId, List<int> sectionId);
    }
}
