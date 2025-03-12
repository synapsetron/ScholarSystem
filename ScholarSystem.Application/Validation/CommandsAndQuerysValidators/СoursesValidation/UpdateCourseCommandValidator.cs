using FluentValidation;
using ScholarSystem.Application.MediatR.Courses.Update;
using ScholarSystem.Application.Validation.DTOValidators.Сourse;

namespace ScholarSystem.Application.Validation.Course
{
    public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseCommandValidator()
        {
            RuleFor(x => x.UpdateCourseDTO)
                .NotNull().WithMessage("UpdateCourseDTO cannot be null.")
                .SetValidator(new UpdateCourseDTOValidator());
        }
    }
}
