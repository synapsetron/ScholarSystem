using FluentValidation;
using ScholarSystem.Application.MediatR.Students.Create;

namespace ScholarSystem.Application.Validation.Student
{
    public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator()
        {
            RuleFor(x => x.studentDTO)
                .NotNull().WithMessage("StudentDTO cannot be null.")
                .SetValidator(new CreateStudentDTOValidator());
        }
    }
}
