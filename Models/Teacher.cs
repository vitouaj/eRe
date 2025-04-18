namespace ERE.Models;

public class Teacher {

    public string Id {get ; set;}
    public string Name { get; set; }
    public string Email {get; set;}
    public string Phone {get; set;}
    public string UserId {get; set;}
    public User User__r {get; set;} = null!;
    public SubjectId SubjectId {get; private set;}
    public Subject? Subject__r {get; set;}
    public Teacher(User user, SubjectId subjectId) {
        Name = user.Firstname + " " + user.Lastname;
        Email = user.Email;
        Phone = user.Phone;
        SubjectId = subjectId;
        User__r = user;
    }

    public Teacher(User user) {
        Name = user.Firstname + " " + user.Lastname;
        Email = user.Email;
        Phone = user.Phone;
    }
    public Teacher() { }
}