
namespace eRe.Classroom;

public class Classroom
{
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public string TeacherId { get; set; }
    public List<Student> Students { get; set; }
    public List<Report> Reports { get; set; }
    public List<SubjectItem> SubjectItems { get; set; }
}
