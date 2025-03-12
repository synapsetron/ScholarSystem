using FluentValidation;
using ScholarSystem.Application.DTO.Course;

namespace ScholarSystem.Application.Validation.DTOValidators.Сourse
{
    public class CreateCourseDTOValidator : AbstractValidator<CreateCourseDTO>
    {
        public CreateCourseDTOValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Course title is required.")
                .MaximumLength(200).WithMessage("Course title must not exceed 200 characters.");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(c => c.Credits)
                .InclusiveBetween(1, 10).WithMessage("Credits must be between 1 and 10.");

            RuleFor(c => c.TeacherId)
                .GreaterThan(0).WithMessage("Valid TeacherId is required.");
        }
    }
}
