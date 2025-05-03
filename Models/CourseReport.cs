
using System.Text.Json.Serialization;
using ERE.DTO;

namespace ERE.Models;


public class CourseReport {
    public string Id {get; set;} = Guid.NewGuid().ToString();
    public string MainReportId { get; set; }
    [JsonIgnore]
    public MainReports MainReport__r { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public MonthId MonthId { get; set;}
    public string CourseId {get; set;}
    public string StudentId { get; set;}
    public string EnrollmentId { get; set;}
    public string Subject {get; set;}

    [JsonIgnore]
    public Enrollment Enrollment__r { get; set; } = null!;
    public float Score { get; set;}
    public int Absences { get; set;}
    public string? TeacherCmt { get; set;}
    public string? ParentCmt { get; set;}
    public GradeId GradeId { get; set;}
    public string StudentName { get; set; }
    public string TeacherName { get; set; }
    public string CourseName { get; set;}
    public StatusId StatusId { get; set; } = StatusId.UNDONE;

    
    public CourseReport(Enrollment enrollment, MonthId monthId) {
        EnrollmentId = enrollment.Id;
        StudentName = enrollment.StudentName;
        TeacherName = enrollment.TeacherName;
        CourseName = enrollment.CourseName;
        CourseId = enrollment.CourseId;
        StudentId = enrollment.StudentId;
        MonthId = monthId;
        Score = 0;
        GradeId = GradeId.F;
        Absences = 0;
        TeacherCmt = "";
        ParentCmt = "";
    }
     public CourseReport(Enrollment enrollment, MonthId monthId, CreateCourseReportDto courseReportDto) {
        EnrollmentId = enrollment.Id;
        StudentName = enrollment.StudentName;
        TeacherName = enrollment.TeacherName;
        CourseName = enrollment.CourseName;
        CourseId = enrollment.CourseId;
        StudentId = enrollment.StudentId;
        MonthId = monthId;
        Score = courseReportDto.Score;
        Absences = courseReportDto.Absences;
        TeacherCmt = courseReportDto.TeacherCmt;
        ParentCmt = "";
    }
    public CourseReport() { }
}

public enum MonthId {
    January = 1,
    February = 2,
    March = 3,
    April = 4,
    May = 5,
    June = 6,
    July = 7,
    August = 8,
    September = 9,
    October = 10,
    November = 11,
    December = 12
}

public enum GradeId {
    A = 1,
    B = 2,
    C = 3,
    D = 4,
    E = 5,
    F = 6
}

public enum StatusId {
    UNDONE = 1,
    DONE = 2,
    Delivered = 3,
}