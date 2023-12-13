using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }

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
            .HasForeignKey(c => c.CreatedBy);

        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedCourses)
            .WithOne(c => c.CreatedBy)
            .HasForeignKey(c => c.CreatedById);
    }
}