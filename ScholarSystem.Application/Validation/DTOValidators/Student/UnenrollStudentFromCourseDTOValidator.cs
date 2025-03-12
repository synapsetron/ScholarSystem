using FluentValidation;
using ScholarSystem.Application.DTO.Student;

namespace ScholarSystem.Application.Validation.Student
{
    public class UnenrollStudentFromCourseDTOValidator : AbstractValidator<UnenrollStudentFromCourseDTO>
    {
        public UnenrollStudentFromCourseDTOValidator()
        {
            Include(new EnrollStudentInCourseDTOValidator());
        }
    }
}
