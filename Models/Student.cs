namespace ERE.Models;

public class Student {
    public string Id {get; set;}
    public string Name {get; set;}
    public string UserId {get; set;}
    public User User__r {get; set;} = null!; // required
}