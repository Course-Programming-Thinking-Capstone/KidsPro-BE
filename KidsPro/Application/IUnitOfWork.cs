using Application.Interfaces.IRepositories;

namespace Application;

public interface IUnitOfWork : IDisposable
{
    public IRoleRepository RoleRepository { get; }
    public IAccountRepository AccountRepository { get; }
    public IParentRepository ParentRepository { get; }
    public IStudentRepository StudentRepository { get; }
    public IStaffRepository StaffRepository { get; }
    public ICourseRepository CourseRepository { get; }
    public ITeacherRepository TeacherRepository { get; }
    public Task<int> SaveChangeAsync();

    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();
}