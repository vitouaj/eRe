using ERE.DTO;
using Microsoft.EntityFrameworkCore;
using ERE.Infrastructure;
using ERE.Models;
using ERE.CustomExceptions;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using System.Net;

namespace ERE.Repository;
public interface ITeacherRepository
{
    Task<Response> CreateCourse(CreateCourseDto request);
    Task<Response> CreateCourseReport(CreateCourseReportDto request);
    Task<Response> UpdateCourseReportsStatus(UpdateCourseReportStatusDto request);
    // Task<Response> SendCourseReportViaEmail(List<CourseReport> courseReports);
}
public class TeacherRepository(AppDbContext context, IMailService mail_service) : ITeacherRepository
{
    private readonly AppDbContext db = context;

    private readonly IMailService Mail_Service = mail_service;

    public async Task<Response> CreateCourse(CreateCourseDto request)
    {
        // check if teacher exists
        var response = new Response();
        var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.UserId == request.TeacherId);
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
        newCourse.CourseDays = request.DayOfWeeks;
        newCourse.CourseTimes = request.TimeOfDays;
        db.Courses.Add(newCourse);
        await db.SaveChangesAsync();

        response.Payload = newCourse;
        response.Message = "Course created successfully";
        response.Success = true;
        return response;

    }

    public async Task<Response> CreateCourseReport(CreateCourseReportDto request)
    {
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
        // before creating course report, create one MainReport if not exists
        var mainReport = await db.MainReports.FirstOrDefaultAsync(mr => mr.StudentId == enrollment.StudentId && mr.MonthId == request.MonthId && mr.LevelId == enrollment.LevelId);
        if (mainReport == null)
        {
            mainReport = new MainReports();
            mainReport.StudentId = enrollment.StudentId;
            mainReport.LevelId = enrollment.LevelId;
            mainReport.MonthId = request.MonthId;
            mainReport.StudentName = enrollment.StudentName;
            mainReport.StudentEmail = enrollment.StudentEmail;
            db.MainReports.Add(mainReport);
            await db.SaveChangesAsync();
        }
        // create course report
        var csReport = new CourseReport(enrollment, request.MonthId);
        csReport.MainReportId = mainReport.Id; // link to main report
        csReport.Score = request.Score;
        csReport.Absences = request.Absences;
        csReport.TeacherCmt = request.TeacherCmt;
        db.CourseReports.Add(csReport);
        await db.SaveChangesAsync();

        // asynchronously send email to student reporting this course performance
        var email = new MailData(){
            EmailToId = mainReport.StudentEmail,
            EmailSubject = "Course Report",
            EmailBody = $"Hello {mainReport.StudentName},\n\nYour course report for {request.MonthId} has been created.\n\n" +
                   $"Score: {csReport.Score}\nAbsences: {csReport.Absences}\nTeacher Comment: {csReport.TeacherCmt}\n\n" +
                   "Best regards,\nERE Team"
        };
        var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525) {
            Credentials = new NetworkCredential("ce2e673c8ab1a3", "0ac071d31fb48e"),
            EnableSsl = true
        };
        client.Send("noreply@ere-reporting.com", email.EmailToId, email.EmailSubject, email.EmailBody);

        response.Payload = csReport;
        response.Message = "Course report created successfully";
        response.Success = true;
        return response;
    }

    public async Task<Response> UpdateCourseReportsStatus(UpdateCourseReportStatusDto request) {
        var response = new Response();
        var courseIds = request.CourseReportIds;
        var courseReports = await db.CourseReports
            .Where(cr => courseIds.Contains(cr.Id))
            .ToListAsync();

        var mainReportIds = courseReports.Select(r => r.MainReportId).ToList();
        var mainReports = await db.MainReports
            .Include(m => m.CourseReports)
            .Where(m => mainReportIds.Contains(m.Id))
            .ToListAsync();

        var mainReportCourseReportMap = mainReports.ToDictionary(
            m => m.Id,
            m => m.CourseReports.ToList()
        );
        var mainReportMap = mainReports.ToDictionary(m => m.Id);

        foreach (var courseReport in courseReports) {
            courseReport.StatusId = (StatusId)request.Status;
        }
        await db.SaveChangesAsync();

        // Recalculate main report status
        foreach (var mainReportId in mainReportCourseReportMap.Keys) {
            var newStatus = RecalculateMainReportStatus(mainReportCourseReportMap[mainReportId]);
            mainReportMap[mainReportId].Status = newStatus;
        }

        await db.SaveChangesAsync();
        response.Payload = mainReportCourseReportMap.Values.ToList();
        response.Message = "Course report status updated successfully";
        response.Success = true;
        return response;
    }

    private ReportStatus RecalculateMainReportStatus(List<CourseReport> courseReports) {
        if (courseReports.IsNullOrEmpty()) {
            return ReportStatus.NOT_READY;
        }
        var allDone = courseReports.All(cr => cr.StatusId == StatusId.DONE);
        return allDone ? ReportStatus.READY : ReportStatus.NOT_READY;
    }
    class CourseReportDto {
        public string Id { get; set; }
        public StatusId StatusId { get; set; }
    }

}

public class UpdateCourseReportStatusDto {
    public List<string> CourseReportIds { get; set; }
    public ReportStatus Status { get; set; }
}
