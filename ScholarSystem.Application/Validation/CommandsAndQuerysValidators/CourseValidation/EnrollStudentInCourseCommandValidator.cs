using FluentValidation;
using ScholarSystem.Application.MediatR.Students.Courses;

namespace ScholarSystem.Application.Validation.Student
{
    public class EnrollStudentInCourseCommandValidator : AbstractValidator<EnrollStudentInCourseCommand>
    {
        public EnrollStudentInCourseCommandValidator()
        {
            RuleFor(x => x.EnrollStudentDTO)
                .NotNull().WithMessage("EnrollStudentDTO cannot be null.")
                .SetValidator(new EnrollStudentInCourseDTOValidator());
        }
    }
}
