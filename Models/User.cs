using System.Text.Json.Serialization;

namespace ERE.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonIgnore]
    public string Firstname { get; set; }
    [JsonIgnore]
    public string Lastname { get; set; }

    [JsonIgnore]
    public string Email { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
    [JsonIgnore]
    public string Phone { get; set; }

    [JsonIgnore]
    public RoleId RoleId { get; set; }

    [JsonPropertyName("RoleId")]
    public string RoleIdString => RoleId.ToString();

    [JsonIgnore]
    public Role? Role__r {get; set;}
}
