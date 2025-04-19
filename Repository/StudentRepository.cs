using ERE.DTO;
using Microsoft.EntityFrameworkCore;
using ERE.Infrastructure;
using ERE.Models;
using ERE.CustomExceptions;

namespace ERE.Repository;
public interface IStudentRepository
{
    Task<Response> EnrollCourse(List<EnrollmentDto> requests);
    Task<Response> UnrollCourse(EnrollmentDto request);
}
public class StudentRepository(AppDbContext context) : IStudentRepository
{
    private readonly AppDbContext db = context;

    public async Task<Response> EnrollCourse(List<EnrollmentDto> requests) {
        var response = new Response();
        var studentIds = requests.Select(r => r.StudentId).ToHashSet();
        var courseIds = requests.Select(r => r.CourseId).ToHashSet();

        var students = await db.Students.Where(s => studentIds.Contains(s.Id)).ToListAsync();
        var courses = await db.Courses.Include(c => c.Teacher__r).Where(c => courseIds.Contains(c.Id)).ToListAsync();

        var foundStudentIds = students.Select(s => s.Id).ToHashSet();
        var foundCourseIds = courses.Select(c => c.Id).ToHashSet();

        var missingStudentIds = studentIds.Except(foundStudentIds).ToHashSet();
        var missingCourseIds = courseIds.Except(foundCourseIds).ToHashSet();

        if (missingStudentIds.Any())
            throw new StudentNotFoundException($"Students not found: {string.Join(", ", missingStudentIds)}");

        if (missingCourseIds.Any())
            throw new CourseNotFoundException($"Courses not found: {string.Join(", ", missingCourseIds)}");

        var existingEnrollments = await db.Enrollments
            .Where(e => studentIds.Contains(e.StudentId) && courseIds.Contains(e.CourseId))
            .ToListAsync();

        var validEnrollments = new List<Enrollment>();

        foreach (var request in requests) {
            var student = students.First(s => s.Id == request.StudentId);
            var course = courses.First(c => c.Id == request.CourseId);

            bool alreadyExists = existingEnrollments.Any(e =>
                e.StudentId == request.StudentId && e.CourseId == request.CourseId);

            if (alreadyExists) {
                throw new EnrollmentAlreadyExistsException($"Student {request.StudentId} already enrolled in course {request.CourseId}");
            }

            var newEnrollment = new Enrollment(student, course) {
                EnrollmentDate = DateTime.UtcNow
            };

            validEnrollments.Add(newEnrollment);
        }

        db.Enrollments.AddRange(validEnrollments);
        await db.SaveChangesAsync();

        response.Payload = new {
            validEnrollments = validEnrollments,
            invalidStudentIds = missingStudentIds,
            invalidCourseIds = missingCourseIds
        };
        response.Message = "Enrollment(s) created successfully";
        response.Success = true;
        return response;
    }


    public async Task<Response> UnrollCourse(EnrollmentDto request)
    {
        FoundEntity found = await throwIfNotFoundAsync(request, db);
        var response = new Response();
        var enrollment = await db.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == request.StudentId && e.CourseId == request.CourseId);
        if (enrollment == null)
        {
            throw new EnrollmentNotFoundException();
        }
        // delete enrollment
        db.Enrollments.Remove(enrollment);
        db.SaveChanges();
        response.Payload = enrollment;
        response.Message = "Enrollment deleted successfully";
        response.Success = true;
        return response;
    }

    private async static Task<FoundEntity> throwIfNotFoundAsync(EnrollmentDto request, AppDbContext db) {
    var student = await db.Students.FirstOrDefaultAsync(s => s.Id == request.StudentId);
    if (student == null) {
        throw new StudentNotFoundException();
    }
    var course = await db.Courses
        .Include(c => c.Teacher__r)
        .FirstOrDefaultAsync(c => c.Id == request.CourseId);
    if (course == null) {
        throw new CourseNotFoundException();
    }
    return new FoundEntity {
        Student = student,
        Course = course
    };
}


    class FoundEntity {
        public Student Student = new Student();
        public Course Course = new Course();
    }
}

[Serializable]
internal class EnrollmentNotFoundException : Exception
{
    public EnrollmentNotFoundException()
    {
    }

    public EnrollmentNotFoundException(string? message) : base(message)
    {
    }

    public EnrollmentNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

[Serializable]
internal class EnrollmentAlreadyExistsException : Exception
{
    public EnrollmentAlreadyExistsException()
    {
    }

    public EnrollmentAlreadyExistsException(string? message) : base(message)
    {
    }

    public EnrollmentAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

[Serializable]
internal class CourseNotFoundException : Exception
{
    public CourseNotFoundException()
    {
    }

    public CourseNotFoundException(string? message) : base(message)
    {
    }

    public CourseNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

[Serializable]
internal class StudentNotFoundException : Exception
{
    public StudentNotFoundException()
    {
    }

    public StudentNotFoundException(string? message) : base(message)
    {
    }

    public StudentNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}