using ERE.DTO;
using Microsoft.EntityFrameworkCore;
using ERE.Infrastructure;
using ERE.Models;
using ERE.CustomExceptions;

namespace ERE.Repository;
public interface ITeacherRepository
{
    Task<Response> CreateCourse(CreateCourseDto request);
    Task<Response> CreateCourseReport(CreateCourseReportDto request);
    Task<Response> SendCourseReportViaEmail(List<CourseReport> courseReports);
    Task<Response> GetMainReports(List<CourseReport> courseReports);

}
public class TeacherRepository(AppDbContext context) : ITeacherRepository
{
    private readonly AppDbContext db = context;

    public async Task<Response> CreateCourse(CreateCourseDto request)
    {
        // check if teacher exists
        var response = new Response();
        var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.Id == request.TeacherId);
        if (teacher == null)
        {
            throw new TeacherNotFoundException();
        }
        // check if course already exists
        var course = await db.Courses.FirstOrDefaultAsync(c => c.TeacherId == request.TeacherId && c.LevelId == request.Level);
        if (course != null)
        {
            throw new CourseAlreadyExistsException();
        }
        // create course
        var newCourse = new Course(teacher, request.Level);
        newCourse.MaxScore = request.MaxScore;
        newCourse.PassingRate = request.PassingRate;
        db.Courses.Add(newCourse);
        await db.SaveChangesAsync();

        response.Payload = newCourse;
        response.Message = "Course created successfully";
        response.Success = true;
        return response;

    }

    public async Task<Response> CreateCourseReport(CreateCourseReportDto request)
    {
        // check if course exists
        var response = new Response();
        
        // check if enrollment exists
        var enrollment = await db.Enrollments.FirstOrDefaultAsync(e => e.Id == request.EnrollmentId);
        if (enrollment == null)
        {
            throw new EnrollmentNotFoundException();
        }
        // check if course report already exists
        var courseReport = await db.CourseReports.FirstOrDefaultAsync(cr => cr.EnrollmentId == request.EnrollmentId && cr.MonthId == request.MonthId);
        if (courseReport != null)
        {
            throw new CourseReportAlreadyExistsException();
        }
        // create course report
        var csReport = new CourseReport(enrollment, request.MonthId);
        csReport.Score = request.Score;
        csReport.Absences = request.Absences;
        csReport.TeacherCmt = request.TeacherCmt;
        db.CourseReports.Add(csReport);
        await db.SaveChangesAsync();

        response.Payload = csReport;
        response.Message = "Course report created successfully";
        response.Success = true;
        return response;
    }

    public async Task<Response> GetMainReport(string studentId, MonthId monthId)
    {
        var response = new Response();
        var courseReports = await db.CourseReports
            .Include(cr => cr.Enrollment__r)
            .ThenInclude(e => e.StudentEmail)
            .Where(cr => cr.StudentId == studentId && cr.MonthId == monthId)
            .ToListAsync();

        if (courseReports.Count == 0)
        {
            response.Success = false;
            response.Message = "No course reports found";
            return response;
        }

        response.Payload = courseReports;
        response.Message = "Course reports found";
        response.Success = true;
        return response;
    }

    public async Task<Response> SendCourseReportViaEmail(List<CourseReport> courseReports)
    {
        var response = new Response();
        // send email
        foreach (var courseReport in courseReports)
        {
            var studentEmail = courseReport.Enrollment__r.StudentEmail;
            // send email to student
            // await EmailService.SendEmail(studentEmail, courseReport);
        }
        response.Message = "Course reports sent successfully";
        response.Success = true;
        return response;
    }
}

[Serializable]
internal class CourseReportAlreadyExistsException : Exception
{
    public CourseReportAlreadyExistsException()
    {
    }

    public CourseReportAlreadyExistsException(string? message) : base(message)
    {
    }

    public CourseReportAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}