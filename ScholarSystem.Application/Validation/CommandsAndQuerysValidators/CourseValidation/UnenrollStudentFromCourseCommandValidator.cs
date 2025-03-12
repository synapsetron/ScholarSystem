using FluentValidation;
using ScholarSystem.Application.MediatR.Students.Courses;

namespace ScholarSystem.Application.Validation.Student
{
    public class UnenrollStudentFromCourseCommandValidator : AbstractValidator<UnenrollStudentFromCourseCommand>
    {
        public UnenrollStudentFromCourseCommandValidator()
        {
            RuleFor(x => x.UnenrollStudentDTO)
                .NotNull().WithMessage("UnenrollStudentDTO cannot be null.")
                .SetValidator(new UnenrollStudentFromCourseDTOValidator());
        }
    }
}
