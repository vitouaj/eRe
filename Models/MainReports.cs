namespace ERE.Models;
using ERE.DTO;

/*
    12 Main report is generated when a user is active in the system.

    It contains the list of course reports for each month.

    It is used to send the report to the student and parent.

    One student can have multiple main reports.

    One main report can have multiple course reports.

    When all course reports of the report is status Done, then the report status == READY.

    When the report is status READY, it can be sent to the student and parent.

    Only report with status READY can be seen by student and parent.

   
*/

public class MainReports {
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public ReportStatus Status { get; set; } = ReportStatus.NOT_READY;
    public string StudentId { get; set; }
    public LevelId LevelId { get; set; }
    public MonthId MonthId { get; set; }
    public string StudentName { get; set; }
    public string StudentEmail { get; set; }
    public string ParentCmt { get; set; } = "";
    public List<CourseReport> CourseReports { get; set; } = new List<CourseReport>();
}

public enum ReportStatus {
    NOT_READY = 0,
    READY = 1,
    FEEDBACK_RECIEVED = 2,
}