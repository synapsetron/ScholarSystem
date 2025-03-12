using FluentValidation;
using ScholarSystem.Application.MediatR.Courses.Create;
using ScholarSystem.Application.Validation.DTOValidators.Сourse;

namespace ScholarSystem.Application.Validation.Course
{
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
            RuleFor(x => x.courseDTO)
                .NotNull().WithMessage("CourseDTO cannot be null.")
                .SetValidator(new CreateCourseDTOValidator());
        }
    }
}
