using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;

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
    public ISectionComponentNumberRepository SectionComponentNumberRepository { get; set; }
    public ILessonRepository LessonRepository { get; set; }
    public IUserNotificationRepository UserNotificationRepository { get; set; }
    public INotificationRepository NotificationRepository { get; set; }
    public Task<int> SaveChangeAsync();

    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();

    public ICertificateRepository CertificateRepository { get; }
    public IOrderRepository OrderRepository { get; }
    public IVoucherRepository VoucherRepository { get; }
    public IOrderDetailRepository OrderDetailRepository { get; }
    public ITransactionRepository TransactionRepository { get; }
    public ISyllabusRepository SyllabusRepository { get; }
    public IPassConditionRepository PassConditionRepository { get; }
    public IScheduleReposisoty ScheduleReposisoty { get; }
    public IClassRepository ClassRepository { get; }
    public IStudentProgressRepository StudentProgressRepository { get; }
    public IStudentOptionRepository StudentOptionRepository { get; }
    public IStudentQuizRepository StudentQuizRepository { get; }
    
    public IQuizRepository QuizRepository { get; }
    public ICourseGameRepository CourseGameRepository { get; }
    public ITeacherProfileRepository TeacherProfileRepository { get; }
}