using Application.Interfaces.IRepositories;

namespace Application;

public interface IUnitOfWork : IDisposable
{
    public IRoleRepository RoleRepository { get; }
    public IAccountRepository AccountRepository { get; }
    public Task<int> SaveChangeAsync();

    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();
}