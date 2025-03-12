using FluentValidation;
using ScholarSystem.Application.MediatR.Courses.Delete;

namespace ScholarSystem.Application.Validation.Course
{
    public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
    {
        public DeleteCourseCommandValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Course ID must be greater than zero.");
        }
    }
}
