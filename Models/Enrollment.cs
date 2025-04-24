/*

    * Created by: [Your Name]
    * Date: [Date]
    * Description: This class represents an enrollment in a course.
    

    Definition: Enrollment is a class that represents a student's enrollment in a course.

    One Student can have multiple enrollments in different courses.

*/

namespace ERE.Models;
public class Enrollment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string StudentEmail {get; set;}
    public string CourseId { get; set; }
    public LevelId LevelId { get; set; }
    public string CourseName { get; set; }
    public string TeacherId { get; set; }
    public string TeacherName { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime? CompletionDate { get; set; }

    public Enrollment(Student student, Course course) {
        StudentId = student.Id;
        StudentName = student.Name;
        StudentEmail = student.Email;
        LevelId = course.LevelId;
        CourseId = course.Id;
        CourseName = course.SubjectId.ToString() + " " + course.LevelId.ToString();
        TeacherId = course.TeacherId;
        TeacherName = course.Teacher__r.Name;
        EnrollmentDate = DateTime.Now;
    }
    public Enrollment() { }
}
