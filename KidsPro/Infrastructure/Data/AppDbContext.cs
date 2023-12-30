using Application.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<CartDetail> CartDetails { get; set; }
    public virtual DbSet<Class> Classes { get; set; }
    public virtual DbSet<ClassSchedule> ClassSchedules { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<CourseSection> CourseSections { get; set; }
    public virtual DbSet<Curriculum> Curricula { get; set; }
    public virtual DbSet<CurriculumCourse> CurriculumCourses { get; set; }
    public virtual DbSet<CurriculumResource> CurriculumResources { get; set; }
    public virtual DbSet<Lesson> Lessons { get; set; }
    public virtual DbSet<LessonResource> LessonResources { get; set; }
    public virtual DbSet<Option> Options { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<Quiz> Quizzes { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<StudentAnswer> StudentAnswers { get; set; }
    public virtual DbSet<StudentAnswerOption> StudentAnswerOptions { get; set; }
    public virtual DbSet<StudentClass> StudentClasses { get; set; }
    public virtual DbSet<StudentQuiz> StudentQuizzes { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }
    public virtual DbSet<TeacherContactInformation> TeacherContactInformations { get; set; }
    public virtual DbSet<TeacherProfile> TeacherProfiles { get; set; }
    public virtual DbSet<TeacherResource> TeacherResources { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<RefreshToken> Tokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedTransactions)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .IsRequired(true);

        modelBuilder.Entity<User>()
            .HasMany(u => u.ProcessedTransactions)
            .WithOne(t => t.Staff)
            .HasForeignKey(t => t.StaffId)
            .IsRequired(false);

        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedCurriculums)
            .WithOne(c => c.CreatedBy)
            .HasForeignKey(c => c.CreatedById);

        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedCourses)
            .WithOne(c => c.CreatedBy)
            .HasForeignKey(c => c.CreatedById);

        modelBuilder.Entity<Course>()
            .HasOne<User>(c => c.ModifiedBy)
            .WithMany()
            .HasForeignKey(c => c.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Curriculum>()
            .HasOne<User>(c => c.ModifiedBy)
            .WithMany()
            .HasForeignKey(c => c.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Curriculum>()
            .HasOne<User>(c => c.ApprovedBy)
            .WithMany()
            .HasForeignKey(c => c.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CartDetail>()
            .HasOne<Course>(ca => ca.Course)
            .WithMany()
            .HasForeignKey(ca => ca.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderDetail>()
            .HasOne<Course>(o => o.Course)
            .WithMany()
            .HasForeignKey(o => o.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        // modelBuilder.Entity<CurriculumCourse>()
        //     .HasKey(cc => new { cc.CurriculumId, cc.CourseId });

        modelBuilder.Entity<CurriculumCourse>()
            .HasOne<Curriculum>(cc => cc.Curriculum)
            .WithMany()
            .HasForeignKey(cc => cc.CurriculumId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CurriculumCourse>()
            .HasOne<Course>(cc => cc.Course)
            .WithMany()
            .HasForeignKey(cc => cc.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Quiz>()
            .HasOne<User>(q => q.CreatedBy)
            .WithMany()
            .HasForeignKey(q => q.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentAnswer>()
            .HasOne<StudentQuiz>(sq => sq.StudentQuiz)
            .WithMany()
            .HasForeignKey(sq => sq.StudentQuizId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentAnswerOption>()
            .HasOne<StudentAnswer>(sao => sao.StudentAnswer)
            .WithMany()
            .HasForeignKey(sao => sao.StudentAnswerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentClass>()
            .HasOne(sc => sc.Class)
            .WithMany()
            .HasForeignKey(sc => sc.ClassId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentClass>()
            .HasOne(sc => sc.Student)
            .WithMany()
            .HasForeignKey(sc => sc.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentQuiz>()
            .HasOne(sq => sq.Student)
            .WithMany()
            .HasForeignKey(sq => sq.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RefreshToken>(x => { x.ToTable("RefreshToken"); });

        //data seeding
        modelBuilder.Entity<Role>().HasData(
            new Role() { Id = 1, Name = Constant.ADMIN_ROLE },
            new Role() { Id = 2, Name = Constant.STAFF_ROLE },
            new Role() { Id = 3, Name = Constant.TEACHER_ROLE },
            new Role() { Id = 4, Name = Constant.PARENT_ROLE }
        );
    }
}