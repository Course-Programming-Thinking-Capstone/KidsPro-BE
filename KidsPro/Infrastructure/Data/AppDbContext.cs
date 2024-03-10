using Application.Configurations;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Admin> Admins { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }
    public virtual DbSet<Staff> Staves { get; set; }
    public virtual DbSet<Parent> Parents { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<TeacherProfile> TeacherProfiles { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Certificate> Certificates { get; set; }
    public virtual DbSet<Class> Classes { get; set; }
    public virtual DbSet<ClassSchedule> ClassSchedules { get; set; }
    public virtual DbSet<CourseResource> CourseResources { get; set; }
    public virtual DbSet<Section> Sections { get; set; }
    public virtual DbSet<Lesson> Lessons { get; set; }
    public virtual DbSet<StudentLesson> StudentLessons { get; set; }
    public virtual DbSet<StudentProgress> StudentProgresses { get; set; }
    public virtual DbSet<Quiz> Quizzes { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<Option> Options { get; set; }
    public virtual DbSet<StudentQuiz> StudentQuizzes { get; set; }
    public virtual DbSet<StudentOption> StudentOptions { get; set; }
    public virtual DbSet<GameVoucher> GameVouchers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<Game> Games { get; set; }
    public virtual DbSet<LevelType> LevelTypes { get; set; }
    public virtual DbSet<GameLevel> GameLevels { get; set; }
    public virtual DbSet<PositionType> PositionTypes { get; set; }
    public virtual DbSet<GameLevelDetail> GameLevelDetails { get; set; }
    public virtual DbSet<GameVersion> GameVersions { get; set; }
    public virtual DbSet<GameLevelModifier> GameLevelModifiers { get; set; }
    public virtual DbSet<MiniGame> MiniGames { get; set; }
    public virtual DbSet<StudentMiniGame> StudentMiniGames { get; set; }
    public virtual DbSet<GamePlayHistory> GamePlayHistories { get; set; }
    public virtual DbSet<GameUserProfile> GameUserProfiles { get; set; }
    public virtual DbSet<GameItem> GameItems { get; set; }
    public virtual DbSet<ItemOwned> ItemOwneds { get; set; }
    public virtual DbSet<SectionComponentNumber> SectionComponentNumbers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasOne<Parent>(s => s.Parent)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TeacherProfile>()
            .HasOne<Teacher>(s => s.Teacher)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Class>()
            .HasOne<Teacher>(s => s.Teacher)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentMiniGame>()
            .HasOne<Student>(s => s.Student)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentProgress>()
            .HasOne<Course>(s => s.Course)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentQuiz>()
            .HasOne<Student>(s => s.Student)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Course>()
            .HasOne<Account>(c => c.CreatedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Course>()
            .HasOne<Account>(c => c.ModifiedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        //data seeding
        modelBuilder.Entity<Role>().HasData(
            new Role() { Id = 1, Name = Constant.AdminRole },
            new Role() { Id = 2, Name = Constant.StaffRole },
            new Role() { Id = 3, Name = Constant.TeacherRole },
            new Role() { Id = 4, Name = Constant.ParentRole },
            new Role() { Id = 5, Name = Constant.StudentRole }
        );

        modelBuilder.Entity<SectionComponentNumber>().HasData(
            new SectionComponentNumber() { Id = 1, SectionComponentType = SectionComponentType.Video, MaxNumber = 5 },
            new SectionComponentNumber() { Id = 2, SectionComponentType = SectionComponentType.Document, MaxNumber = 3 },
            new SectionComponentNumber() { Id = 3, SectionComponentType = SectionComponentType.Quiz, MaxNumber = 1 },
            new SectionComponentNumber() { Id = 4, SectionComponentType = SectionComponentType.Game, MaxNumber = 1 }
        );
    }
}