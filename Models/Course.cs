
/*
    DEFINITION: Course is a class that represents a course taught by a teacher.

    It contains properties such as Id, StudentId, TeacherId, MaxScore, PassingRate,
    GradeId, LevelId, and SubjectId.
*/

using System.Text.Json.Serialization;

namespace ERE.Models;

public class Course {
    public string Id {get; set;} = Guid.NewGuid().ToString();
    public string TeacherId {get; set;}

    [JsonIgnore]
    public Teacher Teacher__r {get; set;} = null!;
    public float MaxScore {get; set;}
    public float PassingRate {get; set;}
    public LevelId LevelId {get; private set;}
    public SubjectId SubjectId {get; set;}
    public Subject? Subject__r {get; set;}

    [JsonIgnore]
    public ICollection<Student> Students {get; set;} = new List<Student>();
    public List<DayOfWeek> CourseDays {get; set;} = new List<DayOfWeek>();
    public List<TimeOfDay> CourseTimes {get; set;} = new List<TimeOfDay>();
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

public enum DayOfWeek {
    MONDAY = 1, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SATURDAY
}

public enum TimeOfDay {
    SIX_SEVEN = 6, SEVEN_EIGHT, EIGHT_NINE, NINE_TEN, TEN_ELEVEN, ELEVEN_TWELTH, TWELTH_ONE, ONE_TWO, TWO_THREE, THREE_FOUR, FOUR_FIVE, FIVE_SIX
}