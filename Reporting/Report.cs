using System.Security.Principal;
using System.Text.Json.Serialization;
using eRe.Classroom;

namespace eRe;

public class Report
{
    public string ReportId { get; set; }
    public string StudentId { get; set; }
    public Student Student { get; set; }
    public string ClassroomId { get; set; }
    public Classroom.Classroom Classroom { get; set; }
    public string IssuedBy { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public bool Accepted { get; set; }
    public bool IsSent { get; set; }
    public float TotalScore { get; set; }
    public GradeLevel OverallGrade { get; set; }
    public int Ranking { get; set; }
    public float Average { get; set; }
    public int Absence { get; set; }
    public int Permission { get; set; }
    public string? TeacherCmt { get; set; }
    public string? ParentCmt { get; set; }
    public MonthOfYear Month { get; set; }

    [JsonIgnore]
    public List<ReportItem> ReportItems { get; set; }
}
