using FluentValidation;
using ScholarSystem.Application.DTO.Teacher;

namespace ScholarSystem.Application.Validation.Teacher
{
    public class CreateTeacherDTOValidator : AbstractValidator<CreateTeacherDTO>
    {
        public CreateTeacherDTOValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("Teacher name is required.")
                .MaximumLength(100).WithMessage("Teacher name must not exceed 100 characters.");

            RuleFor(t => t.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
