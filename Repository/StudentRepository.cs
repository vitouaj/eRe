using ERE.DTO;
using Microsoft.EntityFrameworkCore;
using ERE.Infrastructure;
using ERE.Models;
using ERE.CustomExceptions;
using Microsoft.IdentityModel.Tokens;

namespace ERE.Repository;
public interface IStudentRepository {
    Task<Response> EnrollCourse(List<EnrollmentDto> requests);
    Task<Response> UnrollCourse(EnrollmentDto request);
    Task<Dictionary<string, List<MainReportDto>>> GetMainReports(GetMainReportDto request, Dictionary<string, string>? teacherDict);
    Task<Response> Feedback(ProvideFeedbackDto request);
    string GetStudentId(string userId);

}
public class StudentRepository(AppDbContext context) : IStudentRepository {
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
    public async Task<Dictionary<string, List<MainReportDto>>> GetMainReports(GetMainReportDto request, Dictionary<string, string>? teacherDict) {
        var students = await db.Students
            .Where(s => request.StudentIds.Contains(s.Id))
            .Select(s => new {
                s.Id,
                s.Name,
                s.Email,
                s.Phone,
                s.UserId,
            }).ToListAsync();
        var studentIds = students.Select(s => s.Id).ToHashSet();
        var studentMainReports = new Dictionary<string, List<MainReportDto>>();
        var mainReports = new Dictionary<string, MainReportDto>();
        if (!students.IsNullOrEmpty()) {
            mainReports = await db.MainReports
                .Where(m => studentIds.Contains(m.StudentId) && (m.Status == ReportStatus.READY || m.Status == ReportStatus.FEEDBACK_RECIEVED))
                .ToDictionaryAsync(mr => mr.Id, mr => new MainReportDto {
                    Id = mr.Id,
                    LevelId = mr.LevelId,
                    StudentId = mr.StudentId,
                    Status = mr.Status.ToString(),
                    MonthId = mr.MonthId,
                    StudentName = mr.StudentName,
                    ParentCmt = mr.ParentCmt,
                    StudentEmail = mr.StudentEmail,
                    CourseReports = new HashSet<CourseReportDto>()
                });
            
            var mainReportIds = mainReports.Keys.ToList();
            var courseReports = await db.CourseReports.Where(cr => mainReportIds.Contains(cr.MainReportId))
                .Select(cr => new CourseReportDto {
                    Id = cr.Id,
                    StatusId = cr.StatusId.ToString(),
                    MainReportId = cr.MainReportId,
                    TeacherName = cr.TeacherName,
                    Score = cr.Score,
                    TeacherId = cr.Enrollment__r.TeacherId,
                    Subject = !teacherDict.IsNullOrEmpty() ? teacherDict[cr.Enrollment__r.TeacherId] : null,
                    Absences = cr.Absences,
                    TeacherCmt = cr.TeacherCmt
                }).ToListAsync();

            foreach (var courseReport in courseReports) {
                if (mainReports.ContainsKey(courseReport.MainReportId)) {
                    var mainReport = mainReports[courseReport.MainReportId];
                    mainReport.CourseReports.Add(courseReport);
                }
            }

            foreach (var mainReport in mainReports.Values) {
                if (!studentMainReports.ContainsKey(mainReport.StudentId)) {
                    studentMainReports[mainReport.StudentId] = new List<MainReportDto>();
                }
                studentMainReports[mainReport.StudentId].Add(mainReport);
            }
        }

        if (studentMainReports.IsNullOrEmpty()) {
            return new Dictionary<string, List<MainReportDto>>();
        }
        return studentMainReports;
    }
    public string GetStudentId(string userId)
    {
        var student = db.Students.FirstOrDefault(s => s.UserId == userId);
        if (student == null)
        {
            throw new StudentNotFoundException();
        }
        return student.Id;
    }
    public async Task<Response> Feedback(ProvideFeedbackDto request)
    {
        var response = new Response();
        var mainReport = await db.MainReports
            .FirstOrDefaultAsync(m => m.Id == request.MainReportId);
        if (mainReport == null)
        {
            throw new MainReportNotFoundException();
        }
        mainReport.ParentCmt = request.Feedback;
        mainReport.Status = ReportStatus.FEEDBACK_RECIEVED;
        mainReport.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        response.Payload = mainReport;
        response.Message = "Feedback provided successfully";
        response.Success = true;
        return response;
    }
}

