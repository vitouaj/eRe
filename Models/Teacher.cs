using System.Text.Json.Serialization;

namespace ERE.Models;

public class Teacher {

    public string Id {get ; set;} = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Email {get; set;}
    public string Phone {get; set;}
    public string UserId {get; set;}
    public User User__r {get; set;} = null!;
    [JsonIgnore]
    public SubjectId SubjectId {get; private set;}

    [JsonPropertyName("SubjectId")]
    public string SubjectIdString => SubjectId.ToString();

    [JsonIgnore]
    public Subject? Subject__r {get; set;}
    public Teacher(User user, SubjectId subjectId) {
        Name = user.Firstname + " " + user.Lastname;
        Email = user.Email;
        Phone = user.Phone;
        SubjectId = subjectId;
        User__r = user;
        UserId = user.Id;
    }

    public Teacher(User user) {
        Name = user.Firstname + " " + user.Lastname;
        Email = user.Email;
        Phone = user.Phone;
        User__r = user;
        UserId = user.Id;
    }
    public Teacher() {
     }
}