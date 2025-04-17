namespace ERE.Models;

public class Student
{
    // user Id
    public string Id { get; set; }
    public string Name { get; set;}
    public string UserId {get; set;}
    public User User__r {get; set;}
    public ICollection<Report> Reports { get; } = new List<Report>();
}
