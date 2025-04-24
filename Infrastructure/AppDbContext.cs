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
    public DbSet<MainReports> MainReports { get; set; }

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


        builder.Entity<MainReports>()
            .HasMany(m => m.CourseReports)
            .WithOne()
            .HasForeignKey(r => r.MainReportId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<MainReports>()
            .HasAlternateKey(mr => new { mr.StudentId, mr.MonthId, mr.LevelId });

        builder.Entity<Role>()
            .HasData(
                new Role { Id = RoleId.TEACHER, Name = "Teacher" },
                new Role { Id = RoleId.STUDENT, Name = "Student" },
                new Role { Id = RoleId.PARENT, Name = "Parent" }
            );

        builder.Entity<Subject>()
            .HasData(
                new Subject { Id = SubjectId.MATH, Name = "Math" },
                new Subject { Id = SubjectId.SCIENCE, Name = "Science" },
                new Subject { Id = SubjectId.ENGLISH, Name = "English" },
                new Subject { Id = SubjectId.HISTORY, Name = "History" },
                new Subject { Id = SubjectId.GEOGRAPHY, Name = "Geography" },
                new Subject { Id = SubjectId.PHYSICAL_EDUCATION, Name = "Physical Education" },
                new Subject { Id = SubjectId.ART, Name = "Art" },
                new Subject { Id = SubjectId.MUSIC, Name = "Music" }
            );

            //     MATH = 1, SCIENCE, ENGLISH, HISTORY, GEOGRAPHY, PHYSICAL_EDUCATION, ART, MUSIC

    }
}
