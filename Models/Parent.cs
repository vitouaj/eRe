namespace ERE.Models;


public class Parent {
    public string Id {get ; set;}
    public string Name { get; set; } = string.Empty;
    public string UserId {get; set;}
    public User User__r {get; set;} = null!; // required
}