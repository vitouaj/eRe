namespace ERE.Models;

public class User
{
    public string Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public RoleId RoleId { get; set; }
    public Role? Role__r {get; set;}
}
