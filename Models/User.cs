namespace ERE.Models;

public class User
{
    public string Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; }
    public string Phone { get; set; } = string.Empty;
    public RoleId RoleId { get; set; }
    public Role Role__r {get; set;} = null!; // required
}
