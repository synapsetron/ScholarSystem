using FluentValidation;
using ScholarSystem.Application.DTO.Student;

namespace ScholarSystem.Application.Validation.Student
{
    public class DeleteStudentDTOValidator : AbstractValidator<DeleteStudentDTO>
    {
        public DeleteStudentDTOValidator()
        {
            RuleFor(s => s.Id)
                .GreaterThan(0).WithMessage("Student ID must be greater than zero.");
        }
    }
}
