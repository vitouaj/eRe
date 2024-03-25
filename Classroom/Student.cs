namespace eRe.Classroom;

public class Student
{
    // user Id
    public string StudentId { get; set; }
    public List<Classroom> Classrooms { get; set; }
    public List<Report> Reports { get; set; }
}
