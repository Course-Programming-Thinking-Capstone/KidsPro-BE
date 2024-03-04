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
    public IGameUserProfileRepository GameUserProfileRepository { get; }
    public IGameRepository GameRepository { get; }
    public IGameItemRepository GameItemRepository { get; }
    public IGameLevelRepository GameLevelRepository { get; }
    public IGameLevelModifierRepository GameLevelModifierRepository { get; }
    public IGameLevelDetailRepository GameLevelDetailRepository { get; }
    public IGamePlayHistoryRepository GamePlayHistoryRepository { get; }
    public IGameQuizRoomRepository GameQuizRoomRepository { get; }
    public IGameStudentQuizRepository GameStudentQuizRepository { get; }
    public IGameVersionRepository GameVersionRepository { get; }
    public IItemOwnedRepository ItemOwnedRepository { get; }
    public ILevelTypeRepository LevelTypeRepository { get; }
    public IPositionTypeRepository PositionTypeRepository { get; }
    public ISectionRepository SectionRepository { get; }
    public Task<int> SaveChangeAsync();

    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();
}