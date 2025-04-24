using System.Text.Json.Serialization;

namespace ERE.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Boolean isActive { get; set; } = true;

    [JsonIgnore]
    public string Firstname { get; set; }
    [JsonIgnore]
    public string Lastname { get; set; }

    [JsonIgnore]
    public string Email { get; set; }
    [JsonIgnore]
    public string? Password { get; set; }

    [JsonIgnore]
    public string DefaultPassword {get; set;} = Guid.NewGuid().ToString().Substring(0, 8);
    [JsonIgnore]
    public string Phone { get; set; }

    [JsonIgnore]
    public RoleId RoleId { get; set; }

    [JsonPropertyName("RoleId")]
    public string RoleIdString => RoleId.ToString();

    [JsonIgnore]
    public Role? Role__r {get; set;}
}
