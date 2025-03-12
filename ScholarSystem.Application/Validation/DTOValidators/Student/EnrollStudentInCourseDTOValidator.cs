using FluentValidation;
using ScholarSystem.Application.DTO.Student;

namespace ScholarSystem.Application.Validation.Student
{
    public class EnrollStudentInCourseDTOValidator : AbstractValidator<EnrollStudentInCourseDTO>
    {
        public EnrollStudentInCourseDTOValidator()
        {
            RuleFor(s => s.StudentId)
                .GreaterThan(0).WithMessage("Valid Student ID is required.");

            RuleFor(s => s.CourseId)
                .GreaterThan(0).WithMessage("Valid Course ID is required.");
        }
    }
}
