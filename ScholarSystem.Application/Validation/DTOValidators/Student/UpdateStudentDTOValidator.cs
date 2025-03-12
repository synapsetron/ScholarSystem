using FluentValidation;
using ScholarSystem.Application.DTO.Student;

namespace ScholarSystem.Application.Validation.Student
{
    public class UpdateStudentDTOValidator : AbstractValidator<UpdateStudentDTO>
    {
        public UpdateStudentDTOValidator()
        {
            RuleFor(s => s.Id)
                .GreaterThan(0).WithMessage("Student ID must be greater than zero.");

            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Student name is required.")
                .MaximumLength(100).WithMessage("Student name must not exceed 100 characters.");

            RuleFor(s => s.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(s => s.EnrollmentDate)
                .NotEmpty().WithMessage("Enrollment date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Enrollment date cannot be in the future.");
        }
    }
}
