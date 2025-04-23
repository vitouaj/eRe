using System.Text.Json.Serialization;

namespace ERE.Models;

public class Student {
    public string Id {get; set;} = Guid.NewGuid().ToString();
    public string Name {get; set;}
    public LevelId LevelId  {get; set;}
    public string Email {get; set;}
    public string Phone {get; set;}
    public string UserId {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
    
    [JsonIgnore]
    public User User__r {get; set;} = null!;

    [JsonIgnore]
    public ICollection<Parent> Parents {get; set;} = new List<Parent>();

    [JsonIgnore]
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