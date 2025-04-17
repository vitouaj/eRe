namespace ERE.Models;

public class Teacher {

    public string Id {get ; set;}
    public string Name { get; set; } = string.Empty;
    public string UserId {get; set;}
    public User User__r {get; set;} = null!;
    public SubjectId SubjectId {get; set;}
    public Subject Subject__r {get; set;} = null!;
}