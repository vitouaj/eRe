using System.ComponentModel.DataAnnotations;
using ERE.Models;

namespace ERE.DTO;

public class RegisterRequestDto {
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public LevelId? LevelId { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(20)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    public string Password { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public RoleId Role { get; set; }
    public SubjectId? Subject { get; set; } 
    public List<ContactDto>? Contacts { get; set; } = new List<ContactDto>();
}