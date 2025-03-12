using FluentValidation;
using ScholarSystem.Application.MediatR.Students.Delete;

namespace ScholarSystem.Application.Validation.Student
{
    public class DeleteStudentCommandValidator : AbstractValidator<DeleteStudentCommand>
    {
        public DeleteStudentCommandValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Student ID must be greater than zero.");
        }
    }
}
