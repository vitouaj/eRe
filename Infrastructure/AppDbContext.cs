using ERE.Models;
using Microsoft.EntityFrameworkCore;

namespace ERE.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<CourseReport> CourseReports { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        builder.UseNpgsql("Server=localhost; Port=5009; User Id=postgres; Password=ere.db; Database=ere.db");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /*
            Configure User and Role

            User has one roleds

            Relationship is not defined

            User store string representation of Role (enum)
        */
        builder.Entity<User>()
            .HasOne(u => u.Role__r)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Parent>()
            .HasOne(p => p.User__r)
            .WithOne()
            .HasForeignKey<Parent>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Teacher>()
            .HasOne(t => t.User__r)
            .WithOne()
            .HasForeignKey<Teacher>(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Student>()
            .HasOne(s => s.User__r)
            .WithOne()
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Teacher>()
            .HasOne(t => t.Subject__r)
            .WithMany()
            .HasForeignKey(t => t.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<Parent>()
            .HasMany(p => p.Students)
            .WithMany(s => s.Parents)
            .UsingEntity<Contact>();

        builder.Entity<Course>()
            .HasOne(c => c.Teacher__r)
            .WithMany()
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Student>()
            .HasMany(s => s.Courses)
            .WithMany(c => c.Students)
            .UsingEntity<Enrollment>();

        builder.Entity<Enrollment>()
            .HasAlternateKey(e => new { e.StudentId, e.CourseId });

        builder.Entity<CourseReport>()
            .HasOne(r => r.Enrollment__r)
            .WithMany()
            .HasForeignKey(r => r.EnrollmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}
