using eRe.Classroom;
using eRe.User;
using Microsoft.EntityFrameworkCore;

namespace eRe.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Classroom.Classroom> Classrooms { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<ReportItem> ReportItems { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectItem> SubjectItems { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<User.User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

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

            User has one role

            Relationship is not defined

            User store string representation of Role (enum)
        */

        builder.Entity<User.User>()
            .HasKey(u => u.UserId);

        builder.Entity<User.User>()
            .Property(u => u.RoleId)
            .HasConversion<string>();

        builder.Entity<User.User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.Entity<Role>()
            .HasKey(r => r.Id);


        builder.Entity<Role>()
            .HasData(
                Enum.GetValues(typeof(UserRole))
                    .Cast<UserRole>()
                    .Select(ur => new Role
                    {
                        Id = (int)ur,
                        role = ur.ToString()
                    })
            );

        /*
            Configure Classroom, Student, and Enrollment
        
        */


        builder.Entity<Classroom.Classroom>()
            .HasKey(c => c.Id);

        builder.Entity<Classroom.Classroom>()
            .HasMany(c => c.Students)
            .WithMany(s => s.Classrooms)
            .UsingEntity<Enrollment>();


        builder.Entity<Classroom.Classroom>()
            .HasMany(c => c.SubjectItems)
            .WithOne(c => c.Classroom)
            .HasForeignKey(c => c.ClassroomId);
        /*
            Configure Report, Report Item, Subject, Grade
        */
        builder.Entity<SubjectItem>()
            .HasKey(st => st.Id);
        
        builder.Entity<SubjectItem>()
            .HasAlternateKey(st => new {st.SubjectId, st.ClassroomId});

        builder.Entity<SubjectItem>()
            .Property(s => s.SubjectId)
            .HasConversion<string>();

        builder.Entity<Subject>()
            .HasKey(s => s.Id);

        builder.Entity<Subject>()
            .Property(s => s.Name)
            .HasConversion<string>();

        builder.Entity<Subject>()
            .HasMany(s => s.SubjectItems)
            .WithOne(si => si.Subject)
            .HasForeignKey(si => si.SubjectId);


        builder.Entity<Grade>()
            .HasKey(g => g.Id);

        builder.Entity<Grade>()
            .Property(g => g.Level)
            .HasConversion<string>();

        builder.Entity<Month>()
            .HasKey(m => m.Id);

        builder.Entity<Month>()
            .Property(m => m.Value)
            .HasConversion<string>();


        builder.Entity<ReportItem>()
            .HasKey(ri => ri.ReportItemId);

        builder.Entity<ReportItem>()
            .HasOne(ri => ri.SubjectItem)
            .WithMany(s => s.ReportItems)
            .HasForeignKey(ri => ri.SubjectItemId);

        builder.Entity<Report>()
            .HasKey(r => r.ReportId);

        builder.Entity<Report>()
            .Property(r => r.Month)
            .HasConversion<string>();

        builder.Entity<Report>()    
            .HasMany(r => r.ReportItems)
            .WithOne(ri => ri.Report)
            .HasForeignKey(ri => ri.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        /*
            Configure relationship

            report and student, report and classroom
        
        */

        builder.Entity<Student>()
            .HasMany(s => s.Reports)
            .WithOne(r => r.Student)
            .HasForeignKey(r => r.StudentId);

        builder.Entity<Classroom.Classroom>()
            .HasMany(c => c.Reports)
            .WithOne(r => r.Classroom)
            .HasForeignKey(r => r.ClassroomId);


        /*
            Seed Month, Grade, Subject
        
        */

        builder.Entity<Month>()
            .HasData(
                Enum.GetValues<MonthOfYear>()
                    .Cast<MonthOfYear>()
                    .Select(my => new Month { Id = (int)my, Value = my })
            );

        builder.Entity<Subject>()
            .HasData(
                Enum.GetValues<SubjectType>()
                    .Cast<SubjectType>()
                    .Select(st => new Subject { Id = (int) st, Name = st })
            );

        builder.Entity<Grade>()
            .HasData(
                Enum.GetValues<GradeLevel>()
                    .Cast<GradeLevel>()
                    .Select(gl => new Grade { Id = (int)gl, Level = gl })
            );
    }
}
