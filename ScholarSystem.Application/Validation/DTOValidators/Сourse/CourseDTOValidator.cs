using FluentValidation;
using ScholarSystem.Application.DTO.Course;

namespace ScholarSystem.Application.Validation.DTOValidators.Сourse
{
    public class CourseDTOValidator : AbstractValidator<CourseDTO>
    {
        public CourseDTOValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Course title is required.")
                .MaximumLength(100).WithMessage("Course title must not exceed 100 characters.");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(c => c.Credits)
                .GreaterThan(0).WithMessage("Credits must be greater than zero.")
                .LessThanOrEqualTo(10).WithMessage("Credits cannot exceed 10.");

            RuleFor(c => c.TeacherId)
                .GreaterThan(0).WithMessage("Valid TeacherId is required.");

            RuleFor(c => c.TeacherName)
                .NotEmpty().WithMessage("Teacher name is required.")
                .MaximumLength(100).WithMessage("Teacher name must not exceed 100 characters.");
        }
    }
}
