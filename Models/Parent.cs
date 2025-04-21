using System.Text.Json.Serialization;

namespace ERE.Models;


public class Parent {
    public string Id {get ; set;} = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Email {get; set;}
    public string Phone {get; set;}
    public string UserId {get; set;}

    [JsonIgnore]
    public User User__r {get; set;} = null!;

    [JsonIgnore]
    public ICollection<Student> Students {get; set;} = new List<Student>();
    public Parent(User user) {
        Name = user.Firstname + " " + user.Lastname;
        Email = user.Email;
        Phone = user.Phone;
        UserId = user.Id;
        User__r = user;
    }
    public Parent() {
    }
}