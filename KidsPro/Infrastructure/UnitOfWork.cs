using Application;
using Application.ErrorHandlers;
using Application.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    private readonly ILogger<UnitOfWork> _logger;

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

    public ICertificateRepository CertificateRepository { get; set; }

    public IOrderRepository OrderRepository { get; set; }

    public IVoucherRepository VoucherRepository { get; set; }

    public IOrderDetailRepository OrderDetailRepository { get; set; }

    public ITransactionRepository TransactionRepository { get; set; }
    public ISyllabusRepository SyllabusRepository { get; }
    public IPassConditionRepository PassConditionRepository { get; }
    public IScheduleReposisoty ScheduleReposisoty { get; }
    public IClassRepository ClassRepository { get; }
    public IStudentProgressRepository StudentProgressRepository { get; }
    public IStudentOptionRepository StudentOptionRepository { get; }
    public IStudentQuizRepository StudentQuizRepository { get; }
    public IQuizRepository QuizRepository { get; }

    public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger, IRoleRepository roleRepository,
        IAccountRepository accountRepository, IParentRepository parentRepository, IStudentRepository studentRepository,
        IStaffRepository staffRepository, ITeacherRepository teacherRepository, ICourseRepository courseRepository,
        IGameUserProfileRepository gameUserProfileRepository, IGameRepository gameRepository,
        IGameItemRepository gameItemRepository, IGameLevelRepository gameLevelRepository,
        IGameLevelModifierRepository gameLevelModifierRepository, IGameLevelDetailRepository gameLevelDetailRepository,
        IGamePlayHistoryRepository gamePlayHistoryRepository, IGameQuizRoomRepository gameQuizRoomRepository,
        IGameStudentQuizRepository gameStudentQuizRepository, IGameVersionRepository gameVersionRepository,
        IItemOwnedRepository itemOwnedRepository, ILevelTypeRepository levelTypeRepository,
        IPositionTypeRepository positionTypeRepository, ISectionRepository sectionRepository, ISectionComponentNumberRepository sectionComponentNumberRepository, ILessonRepository lessonRepository
        , ICertificateRepository certificateRepository, IOrderRepository orderRepository, IVoucherRepository voucherRepository, IOrderDetailRepository orderDetailRepository
        ,  IUserNotificationRepository userNotificationRepository, INotificationRepository notificationRepository, ITransactionRepository transactionRepository, ISyllabusRepository syllabusRepository, IPassConditionRepository passConditionRepository, IScheduleReposisoty scheduleReposisoty, IClassRepository classRepository, IStudentProgressRepository studentProgressRepository, IStudentOptionRepository studentOptionRepository, IStudentQuizRepository studentQuizRepository, IQuizRepository quizRepository)
    {
        _context = context;
        _logger = logger;
        RoleRepository = roleRepository;
        AccountRepository = accountRepository;
        ParentRepository = parentRepository;
        StudentRepository = studentRepository;
        StaffRepository = staffRepository;
        TeacherRepository = teacherRepository;
        CourseRepository = courseRepository;
        GameUserProfileRepository = gameUserProfileRepository;
        GameRepository = gameRepository;
        GameItemRepository = gameItemRepository;
        GameLevelRepository = gameLevelRepository;
        GameLevelModifierRepository = gameLevelModifierRepository;
        GameLevelDetailRepository = gameLevelDetailRepository;
        GamePlayHistoryRepository = gamePlayHistoryRepository;
        GameQuizRoomRepository = gameQuizRoomRepository;
        GameStudentQuizRepository = gameStudentQuizRepository;
        GameVersionRepository = gameVersionRepository;
        ItemOwnedRepository = itemOwnedRepository;
        LevelTypeRepository = levelTypeRepository;
        PositionTypeRepository = positionTypeRepository;
        SectionRepository = sectionRepository;
        SectionComponentNumberRepository = sectionComponentNumberRepository;
        LessonRepository = lessonRepository;
        CertificateRepository = certificateRepository;
        OrderRepository = orderRepository;
        VoucherRepository = voucherRepository;
        OrderDetailRepository = orderDetailRepository;

        UserNotificationRepository = userNotificationRepository;
        NotificationRepository = notificationRepository;
        TransactionRepository = transactionRepository;
        SyllabusRepository = syllabusRepository;
        PassConditionRepository = passConditionRepository;
        ScheduleReposisoty = scheduleReposisoty;
        ClassRepository = classRepository;
        StudentProgressRepository = studentProgressRepository;
        StudentOptionRepository = studentOptionRepository;
        StudentQuizRepository = studentQuizRepository;
        QuizRepository = quizRepository;
    }

    public async Task<int> SaveChangeAsync()
    {
        //Handle concurrency update
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync();

                if (databaseValues != null)
                {
                    // Refresh original values to bypass next concurrency check
                    entry.OriginalValues.SetValues(databaseValues);
                }
                else
                {
                    // Handle entity not found in the database
                    throw new NotFoundException("Entity not found in the database.");
                }
            }

            // Try saving changes again after resolving conflicts
            return await _context.SaveChangesAsync();
        }
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            if (_transaction == null)
                throw new Exception("Transaction is not initiate");
            await _transaction.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error when commit transaction.\nDate:{}", DateTime.UtcNow);
            throw new Exception("Transaction has not been created yet.");
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            if (_transaction == null)
                throw new Exception("Transaction is not initiate");
            await _transaction.RollbackAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error when commit transaction.\nDate:{}", DateTime.UtcNow);
            throw new Exception("Transaction has not been created yet.");
        }
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}