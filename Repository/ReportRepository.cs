using System.Data.Common;
using System.Text.Json.Serialization;
using eRe.Dto;
using eRe.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql.Internal;

namespace eRe.Repository;

public interface IReportRepository
{
    Task<Response> CreateAsync(string teacherId, ReportDto reportDto);
    Task<Response> GetReportByIdAsync(string reportId);
    Task<Response> GetAllAsync(string classId, MonthOfYear? filterMonth);
}

public class ReportRepository(AppDbContext context) : IReportRepository
{
    private readonly AppDbContext db = context;
    public async Task<Response> CreateAsync(string teacherId, ReportDto reportDto)
    {
        var response = new Response();
        var report = new Report
        {
            ReportId = Utilities.GenerateReportId(),
            StudentId = reportDto.StudentId,
            ClassroomId = reportDto.ClassroomId,
            TotalScore = reportDto.TotalScore,
            Average = reportDto.Average,
            OverallGrade = reportDto.OverallGrade,
            Month = reportDto.Month,
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

        try
        {
            await db.Reports.AddAsync(report);
            await db.SaveChangesAsync();

            response.Success = true;
            response.Message = "Create report successfully!";
            response.Payload = report;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<Response> GetAllAsync(string classId, MonthOfYear? filterMonth)
    {
        var response = new Response();

        var query = db.Reports
            .Include(c => c.ReportItems)
            .Where(r => r.ClassroomId.Equals(classId));

        if (filterMonth.HasValue)
        {
            query = query.Where(r => r.Month == filterMonth.Value);
        }

        var results = await query
            .Select(m => new
            {
                m.ReportId,
                Month = ((MonthOfYear)m.Month).ToString(),
                m.StudentId,
                StudentFirstname = db.Users.SingleOrDefault(u => u.UserId == m.StudentId).Firstname,
                StudentLastname = db.Users.SingleOrDefault(u => u.UserId == m.StudentId).Lastname,
                m.ClassroomId,
                m.Classroom.Name,
                ReportItems = m.ReportItems.Select(e =>
                new
                {
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
            .ToListAsync();

        response.Success = true;
        response.Payload = results;

        return response;
    }

    public async Task<Response> GetReportByIdAsync(string reportId)
    {
        var response = new Response();
        try
        {
            var result = await db.Reports
                .Include(c => c.ReportItems)
                .Where(c => c.ReportId == reportId)
                .Select(m => new
                {
                    m.ReportId,
                    Month = m.Month,
                    m.StudentId,
                    StudentFirstname = db.Users.SingleOrDefault(u => u.UserId == m.StudentId).Firstname,
                    StudentLastname = db.Users.SingleOrDefault(u => u.UserId == m.StudentId).Lastname,
                    m.ClassroomId,
                    m.Classroom.Name,
                    m.OverallGrade,
                    ReportItems = m.ReportItems.Select(e =>
                    new
                    {
                        Subject = e.SubjectItem.Subject.Name.ToString(),
                        e.SubjectItem.MaxScore,
                        e.Score,
                        Grade = e.GradeId
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
            .SingleOrDefaultAsync();

            response.Success = true;
            response.Payload = result;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

}


public record ReportDto
(
    float Average,
    string ClassroomId,
    string StudentId,
    MonthOfYear Month,
    string IssuedBy,
    float TotalScore,
    GradeLevel OverallGrade,
    int Absence,
    int Permission,
    string? TeacherCmt,
    List<ReportItemDto> ReportItems
);


public record ReportItemDto
(
    int SubjectItemId,
    float Score,
    GradeLevel GradeId
);