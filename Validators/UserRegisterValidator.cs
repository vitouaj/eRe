
using ERE.DTO;
using ERE.Models;
using FluentValidation;

namespace ERE.Validators;

public class UserRegisterValidator : AbstractValidator<RegisterRequestDto> {
    public UserRegisterValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");
        
        RuleFor(x => x.LevelId)
            .IsInEnum().WithMessage("Invalid level value. Level must be either 1 (PRIMARY) or 2 (SECONDARY) or 3 (TERTIARY).")
            .When(x => x.Role == RoleId.STUDENT)
            .WithMessage("Level is required for students.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .Length(5, 50).WithMessage("Email must be between 5 and 50 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 20).WithMessage("Password must be between 8 and 20 characters.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number format.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .IsInEnum().WithMessage("Invalid role value. Role must be either 1 (STUDENT) or 2 (TEACHER) or 3 (PARENT).");

        RuleFor(x => x.Subject)
            .IsInEnum().WithMessage("Invalid subject value. Subject must be either 1 (MATH) or 2 (ENGLISH) or 3 (SCIENCE).")
            .When(x => x.Role == RoleId.TEACHER)
            .WithMessage("Subject is required for teachers.");

        RuleForEach(x => x.Contacts)
    .ChildRules(contacts => {
        contacts.RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

        contacts.RuleFor(c => c.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

        contacts.RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .Length(5, 50).WithMessage("Email must be between 5 and 50 characters.");

        contacts.RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number format.");

        contacts.RuleFor(c => c.HomeNumber)
            .NotEmpty().WithMessage("Home number is required.");

        contacts.RuleFor(c => c.Street)
            .NotEmpty().WithMessage("Street is required.");

        contacts.RuleFor(c => c.Village)
            .NotEmpty().WithMessage("Village is required.");

        contacts.RuleFor(c => c.Commune)
            .NotEmpty().WithMessage("Commune is required.");

        contacts.RuleFor(c => c.District)
            .NotEmpty().WithMessage("District is required.");

        contacts.RuleFor(c => c.Province)
            .NotEmpty().WithMessage("Province is required.");
    })
    .When(x => x.Role == RoleId.STUDENT); // ✅ Apply this AFTER RuleForEach

    }
}