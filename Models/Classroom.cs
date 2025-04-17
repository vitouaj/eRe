
using ERE.Models;

namespace ERE.Models;

public class Classroom
{
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TeacherId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Student> Student__r { get; set; }
    public List<Report> Report__r { get; set; }
    public List<SubjectItem> SubjectItems { get; set; }

}
