
/*
    DEFINITION: Course is a class that represents a course taught by a teacher.

    It contains properties such as Id, StudentId, TeacherId, MaxScore, PassingRate,
    GradeId, LevelId, and SubjectId.
*/

namespace ERE.Models;

public class Course {
    public string Id {get; set;} = Guid.NewGuid().ToString();
    public string TeacherId {get; set;}
    public Teacher Teacher__r {get; set;} = null!;
    public float MaxScore {get; set;}
    public float PassingRate {get; set;}
    public LevelId LevelId {get; private set;}
    public SubjectId SubjectId {get; set;}
    public Subject? Subject__r {get; set;}
    public ICollection<Student> Students {get; set;} = new List<Student>();
    public Course(Teacher teacher) {
        SubjectId = teacher.SubjectId;
        TeacherId = teacher.Id;
        Teacher__r = teacher;
    }
    public Course(Teacher teacher, LevelId levelId) {
        LevelId = levelId;
        SubjectId = teacher.SubjectId;
        TeacherId = teacher.Id;
        Teacher__r = teacher;
    }
    public Course() {}
}

public enum LevelId {
    SEVENTH = 7, EIGHTH, NINTH, TENTH, ELEVENTH, TWELFTH
}