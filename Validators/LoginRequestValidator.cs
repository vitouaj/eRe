
using ERE.DTO;
using ERE.Models;
using FluentValidation;

namespace ERE.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.EmailOrPhoneNumber)
            .NotEmpty().WithMessage("Email or phone number is required.");


        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 20).WithMessage("Password must be between 8 and 20 characters.");
            
    }
}