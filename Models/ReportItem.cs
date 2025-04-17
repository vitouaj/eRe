using ERE.Models;

namespace ERE.Models;

public class ReportItem
{
    public Guid ReportItemId { get; set; }
    public string ReportId { get; set; }
    public Report Report { get; set; }
    public int SubjectItemId { get; set; }
    public SubjectItem SubjectItem { get; set; }
    public float Score { get; set; }
    public GradeLevel GradeId { get; set; }
}
public class SubjectItem
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
    public float MaxScore { get; set; }
    public float PassingScore { get; set; }
    public List<ReportItem> ReportItems { get; set; }
    public string ClassroomId { get; set; }
    public Classroom Classroom { get; set; }
}

public class Subject
{
    public int Id { get; set; }
    public SubjectType Name { get; set; }
    public List<SubjectItem> SubjectItems { get; set; } = null!;
}

public enum SubjectType
{
    MATH = 1, KHMER, HISTORY, BIOLOGY, PHYSIC, CHEMISTRY, ENGLISH, ECONOMY, GEOGRAPHY
}

public enum GradeLevel
{
    A = 1, B, C, D, E, F
}

public class Grade
{
    public int Id { get; set; }
    public GradeLevel Level { get; set; }
    public string? Detail { get; set; }
}

public class Month
{
    public int Id { get; set; }
    public MonthOfYear Value { get; set; }
}

public enum MonthOfYear
{
    JANUARY = 1, FEBRUARY, MARCH, APRIL, MAY, JUNE, JULY, ARGUST, SEPTEMBER, OCTOBER, NOVEMBER, DECEMBER
}
