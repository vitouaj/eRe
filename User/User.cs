namespace eRe.User;

public class User
{
    public string UserId { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string OtherContact { get; set; } = string.Empty;
    public string? ProfileId { get; set; }
    public UserRole RoleId { get; set; }
}
