using System.Data.Common;
using System.Text.Json.Serialization;
using eRe.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql.Internal;

namespace eRe.Repository;

public interface IReportRepository
{
    Task<object> CreateAsync(string teacherId, ReportDto reportDto);

    Task<object> GetAllAsync(string classId);
}

public class ReportRepository(AppDbContext context) : IReportRepository
{
    private readonly AppDbContext db = context;
    public async Task<object> CreateAsync(string teacherId, ReportDto reportDto)
    {
        var report = new Report
        {
            ReportId = Utilities.GenerateReportId(),
            StudentId = reportDto.StudentId,
            ClassroomId = reportDto.ClassroomId,
            Month = (MonthOfYear)reportDto.Month,
            IssuedBy = teacherId,
            IssuedAt = DateTime.Now,
            Absence = reportDto.Absence,
            Permission = reportDto.Permission,
            TeacherCmt = reportDto.TeacherCmt,
        };

        List<ReportItem> reportItems = [];

        float totalScore = 0;

        foreach (var ri in reportDto.ReportItems)
        {
            var reportItem = new ReportItem
            {
                ReportId = report.ReportId,
                SubjectItemId = ri.SubjectItemId,
                Score = ri.Score,
                GradeId = (GradeLevel)ri.GradeId
            };

            totalScore += ri.Score;

            reportItems.Add(reportItem);
        }

        report.ReportItems = reportItems;
        report.TotalScore = totalScore;
        report.Average = totalScore / reportItems.Count;
        report.Accepted = false;
        report.IsSent = false;

        await db.Reports.AddAsync(report);
        await db.SaveChangesAsync();

        var json = JsonConvert.SerializeObject(report, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

        return json;
    }

    public async Task<object> GetAllAsync(string classId)
    {
        var results = await db.Reports
            .Include(c => c.ReportItems)
            .Where(r => r.ClassroomId.Equals(classId))
            .Select(m => new {
                m.ReportId,
                Month = ((MonthOfYear)m.Month).ToString(),
                m.StudentId,
                m.ClassroomId,
                m.Classroom.Name,
                ReportItems = m.ReportItems.Select(e => 
                new {
                    Subject = e.SubjectItem.Subject.Name.ToString(),
                    e.SubjectItem.MaxScore,
                    e.Score,
                    Grade = ((GradeLevel)e.GradeId).ToString(),
                }),
                m.Absence,
                m.Permission,
                m.TotalScore,
                m.Average,
                m.TeacherCmt,
                m.ParentCmt,
                m.Classroom.TeacherId,
                m.IssuedBy,
                m.IssuedAt,
            })
            .FirstOrDefaultAsync();

        var json = JsonConvert.SerializeObject(results);

        return json;
    }
}


public record ReportDto
(
    string ClassroomId,
    string StudentId,
    int Month,
    string IssuedBy,
    string IssuedAt,
    int Absence,
    int Permission,
    string? TeacherCmt,
    List<ReportItemDto> ReportItems
);


public record ReportItemDto
(
    int SubjectItemId,
    float Score,
    int GradeId
);