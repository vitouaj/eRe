using ERE.Models;
public class CreateCourseReportDto {
    public string EnrollmentId { get; set; }
    public MonthId MonthId { get; set; }
    public float Score { get; set; }
    public int Absences { get; set; }
    public string? TeacherCmt { get; set; }
}