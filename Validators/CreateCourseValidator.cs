
using System.Data;
using ERE.DTO;
using ERE.Models;
using FluentValidation;

namespace ERE.Validators;

public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseValidator()
    {
        RuleFor(x => x.TeacherId)
            .NotEmpty().WithMessage("Teacher ID is required.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level is required.")
            .IsInEnum().WithMessage("Invalid level value");

        RuleFor(x => x.MaxScore)
            .NotEmpty().WithMessage("Max score is required.")
            .GreaterThan(0).WithMessage("Max score must be greater than 0.");

        RuleFor(x => x.PassingRate)
            .NotEmpty().WithMessage("Passing rate is required.")
            .InclusiveBetween(0, 100).WithMessage("Passing rate must be between 0 and 100.");
    }
}