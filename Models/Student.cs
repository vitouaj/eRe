namespace ERE.Models;

public class Student {
    public string Id {get; set;} = Guid.NewGuid().ToString();
    public string Name {get; set;}
    public string Email {get; set;}
    public string Phone {get; set;}
    public string UserId {get; set;}
    public User User__r {get; set;} = null!;
    public ICollection<Parent> Parents {get; set;} = new List<Parent>();
    public ICollection<Course> Courses {get; set;} = new List<Course>();
    public Student(User user) {
        Name = user.Firstname + " " + user.Lastname;
        Email = user.Email;
        Phone = user.Phone;
        UserId = user.Id;
        User__r = user;
    }
    public Student() {
    }
}