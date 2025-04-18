namespace ERE.Models;


public class Parent {
    public string Id {get ; set;}
    public string Name { get; set; }
    public string Email {get; set;}
    public string Phone {get; set;}
    public string UserId {get; set;}
    public User User__r {get; set;} = null!;
    public ICollection<Student> Students {get; set;} = new List<Student>();
    public Parent(User user) {
        Name = user.Firstname + " " + user.Lastname;
        Email = user.Email;
        Phone = user.Phone;
    }
    public Parent() { }
}