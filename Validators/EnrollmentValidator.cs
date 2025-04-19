
using System.Data;
using ERE.DTO;
using ERE.Models;
using FluentValidation;

namespace ERE.Validators;

public class EnrollmentValidator : AbstractValidator<EnrollmentDto>
{
    public EnrollmentValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Student ID is required.");

        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course ID is required.");
    }
}