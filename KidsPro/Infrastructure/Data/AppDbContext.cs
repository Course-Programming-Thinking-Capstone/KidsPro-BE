using Application.Configurations;
using Domain.Entities;
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
    public virtual DbSet<Answer> Answers { get; set; }
    public virtual DbSet<StudentQuiz> StudentQuizzes { get; set; }
    public virtual DbSet<StudentAnswer> StudentAnswers { get; set; }
    public virtual DbSet<Voucher> Vouchers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<Game> Games { get; set; }
    public virtual DbSet<LevelType> LevelTypes { get; set; }
    public virtual DbSet<GameLevel> GameLevels { get; set; }
    public virtual DbSet<PositionType> PositionTypes { get; set; }
    public virtual DbSet<GameLevelDetail> GameLevelDetails { get; set; }
    public virtual DbSet<GameVersion> GameVersions { get; set; }
    public virtual DbSet<GameLevelModifier> GameLevelModifiers { get; set; }
    public virtual DbSet<GameQuizRoom> GameQuizRooms { get; set; }
    public virtual DbSet<GameStudentQuiz> GameStudentQuizzes { get; set; }
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

        modelBuilder.Entity<GameStudentQuiz>()
            .HasOne<Student>(s => s.Student)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentProgress>()
            .HasOne<Course>(s => s.Course)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderDetail>()
            .HasOne<Student>(o => o.Student)
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
    }
}