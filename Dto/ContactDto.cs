using System.ComponentModel.DataAnnotations;
using ERE.Models;

namespace ERE.DTO
{
    public class ContactDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string HomeNumber { get; set; }
        public string Street { get; set; }
        public string Village { get; set; }
        public string Commune { get; set; }
        public string District { get; set; }
        public string Province { get; set; }

    }

    public class CourseReportDto {
        public string Id { get; set; }
        public string MainReportId { get; set; }
        public string StatusId { get; set; }
        public string TeacherName {get; set;}
        public float Score { get; set; }
        public int Absences {get; set;}
        public string? Subject {get; set;}
        public string TeacherId {get; set;}
        public string TeacherCmt { get; set; }
    }

    public class MainReportDto {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public MonthId MonthId { get; set; }
        public LevelId LevelId { get; set; }
        public string Status { get; set; }
        public string ParentCmt { get; set; }
        public HashSet<CourseReportDto> CourseReports { get; set; } = new();

        // Recalculated total absence
        public int TotalAbsence => CourseReports?.Sum(r => r.Absences) ?? 0;
        public float TotalScore => CourseReports?.Sum(r => r.Score) ?? 0;

        // Recalculated total score (e.g., average)
        public float AverageScore => CourseReports != null && CourseReports.Count > 0
            ? CourseReports.Average(r => r.Score)
            : 0f;

        // Recalculated overall grade (e.g., average or based on rules)
        public string OverallGrade
        {
            get
            {
                if (CourseReports == null || CourseReports.Count == 0)
                    return "N/A";

                var average = AverageScore;

                // Example logic
                if (average >= 90) return GradeId.A.ToString();
                if (average >= 80) return GradeId.B.ToString();
                if (average >= 70) return GradeId.C.ToString();
                if (average >= 60) return GradeId.D.ToString();
                if (average >= 50) return GradeId.E.ToString();
                return GradeId.F.ToString();
            }
        }
    }

    public class FoundEntity {
        public Student Student = new Student();
        public Course Course = new Course();
    }

    public class GetMainReportDto {
        public string StudentId { get; set; }
        public List<string> StudentIds {get; set;}
        public MonthId? MonthId { get; set; }
        public LevelId? LevelId { get; set; }
    }


    public class ProvideFeedbackDto {
        public string MainReportId { get; set; }
        public string Feedback { get; set; }
    }

    public class UpdateUserDto {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}