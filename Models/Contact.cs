namespace ERE.Models;

public class Contact {
    public string Id {get; set;} = Guid.NewGuid().ToString();
    public string StudentId {get; set;}
    public string ParentId {get; set;}
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string HomeNumber { get; set; }
    public string Street { get; set; }
    public string Village { get; set; }
    public string Commune { get; set; }
    public string District { get; set; }
    public string Province { get; set; }
}